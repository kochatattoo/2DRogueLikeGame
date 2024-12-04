Для начала создаем внутри объекта ActiveWeapon новый Game Object -> WitchAttack

Давайте напишем скрипт отвечающий за логику атаки магией

```
using System;
using UnityEngine;

public class WitchAttack : MonoBehaviour
{
    //Создаем переменную количество урона 
    [SerializeField] private int _damageAmount = 5;
    //Вводим переменную полигон колайдера 2д
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        //Инициализируем полигонколайдер (кешируем)
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _polygonCollider2D.enabled = false;
    }

    private void Start()
    {
        //Выключаем полигонколайдер в самом начале
        AttackCollaiderTurnOff();
    }

    //Метод, вызывающий атаку
    public void Attack()
    {
        //Включаем полигонколайдер2д при атаке
        AttackCollaiderOffOn();
        
    }

    //Метод, для проверки не коcyулся ли колайдер, другого колайдера в 2д
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Проверяем что ударили по врагу
        //Возвращаем в строку значение типа EnemyEntity из-за out
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            //Вызываем метод ПолучениеУрона у enemyEntity 
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    //Метод, выключающий полигонколайдер2д
    public void AttackCollaiderTurnOff()
    {
        //Выключаем Полигонколайдер2Д
        _polygonCollider2D.enabled = false;
    }
    //Метод, включающий полигонколайдер2д
    private void AttackCollaiderTurnOn()
    {
        //Включаем полигонколайдер2Д
        _polygonCollider2D.enabled = true;
    }

    //Метод для того, что бы при быстром нажатии и взмахе
    //Успевал реализовываться удар по противнику, даже если 
    //анимация не успела закончится и началась следующая
    private void AttackCollaiderOffOn()
    {
        AttackCollaiderTurnOff();
        AttackCollaiderTurnOn();
    }
}

```
Далее нам необходимо создать анимацию и присвоить ей данную логику 

В папку Animations->Player->добавляю анимацию Add animation (MagicAttack)
В аниматоре в графе параметры добавляем Trigger "MagicAttack"\
Переносим анимацию "MagicAttack" в поле аниматора и соединяю ее с состояниями "AnyStates -> MagicAttack -> Idle" 
От AnyStates выставляем в List ->Trigger MagicAttack

![[Pasted image 20241112183058.png]]
Далее переходим в настройку самой анимации спрайта 

Логика создания анимации MagicAttack отличается от реализации меча, поскольку меч у нас является отдельными спрайтом вращающимся вокруг игрока, в то время как анимация магической атаки является анимацией персонажа 
Поэтому дописывать часть кода мы будем в данного персонажа в PlayerVisual

Для начала добавим логику атаки правой кнопкой мыши через новый пункт управления InputAction New
В уже созданое поле Combat -> Add action -> Right Button и назовем наше действие Magic_Attack

![[Pasted image 20241112183535.png]]

После направляемся в скрипт GameInput ит добавляем следующую часть кода
```
 ...
 public event EventHandler OnPlayerMagicAttack;

 private void Awake()
 {
    ...
     _playerInputActions.Combat.Magic_Attack.started += Magic_Attack_started;
    ...
 }
  //Событие атаки магией
 private void Magic_Attack_started(InputAction.CallbackContext obj)
 {
     OnPlayerMagicAttack?.Invoke(this, EventArgs.Empty);
 }
```
Теперь из GameInput мы сможем вызывать событие нашей атаки магией и нанести урон попавшемуся под раздачу 
В скрипт Player необходимо внести следующие дополнение
```
using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    //Объявляем класс статическим и делаем из него СинглТон
    public static Player Instance {  get; private set; }
    //Объявляем события Смерти и получения урона
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnTakeHit;

    //Объявляем переменные
    //Скорость, макс здоровье, время востановления для получения урона, место нахождения
    [SerializeField] private float _speed = 15.0f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;
    Vector2 _inputVector;

    //Переменные RigidBody (физика) и класс отвечающий за отталкивание при получении урона
    private Rigidbody2D _rb;
    private knockBack _knockBack;

    //Минимальная скорость передвижения и статус бега = фолс
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;


    //текущее здоровье, возможно ли получать урон, статус жизни
    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive = true;

    //Переменная отвечающая за хэлфбар
    private PlayerHealthManager _healthBar;

    void Awake()
    {
        //Инициализируем синглтон
       Instance = this;
        //Кешируем компоненты
        _rb= GetComponent<Rigidbody2D>();
        _knockBack=GetComponent<knockBack>();
       
    }

    private void Start()
    {
        //Устанавливаем текущее здоровье = максимальноиу
        _currentHealth=_maxHealth;
        //Может получать урон
        _canTakeDamage=true;
        //Подписываемся на события атаки 
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        GameInput.Instance.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;
    }

  
    private void Update()
    {
        //Отслеживание вектора персонажа
        _inputVector = GameInput.Instance.GetMovementVector();
        // Debug.Log(GameManager.Instance.user.GetName());
    
    }

    void FixedUpdate()
    {
        //Проверяем находимлся ли мы в состоянии отлета
        if (_knockBack.IsGettingKnockedBack)
            return;
        //метод проверяющий статус бега
        HandleMovement();
    }

    //Событие атаки 
    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        //Вызываем метод в атаки в классе Актив Вепон
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
 
    }
    //Событие магической атаки
    private void Player_OnPlayerMagicAttack(object sender, EventArgs e)
    {
        //Вызываем метод в атаки в классе Актив Вепон
        ActiveWeapon.Instance.GetMagicWeapon().Attack();

    }

    //Отслеживание статуса бега персонажа
    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * _speed * Time.fixedDeltaTime);

        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed){
            _isRunning = true;
        } else{
            _isRunning= false;
        }
    }

    //Возвращаем статус нашего персонажа и получение закрытых переменных
    public bool IsRunning() { return _isRunning; }
    public bool IsAlive() =>_isAlive;
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
    public float GetMaxHealth()
    {
        return _maxHealth;
    }
    //Получение позиции нашего персонажа относительно экрана
    public Vector3 GetPlayerScreenPosiyion()
    {
        Vector3 playerScreenPosition=Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    //Метод вызываемый при получении урона главным героем
    public void TakeDamage(Transform damageSourse, int damage)
    {
        //Если он может получать урон
        if (_canTakeDamage&&_isAlive)
        {
            //Высчитываем текущее здоровье в диапазоне 0 и макс здоровья
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            //Вызываем метод отскакивания при получении урона
            _knockBack.GetKnockBack(damageSourse);
            //Больше не можем получать урон
            _canTakeDamage = false;
            OnTakeHit?.Invoke(this, EventArgs.Empty);
            //Начинаем отчет до следующей возможности получать урон
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log(_currentHealth);
        }
        //Отслеживае состояние смерти
        DetectDeath();
    }
    //Метод отслеживающий состояние смерти героя
    private void DetectDeath()
    {
        //Если текущее здоровье равно 0, то останавливаем время
        if (_currentHealth == 0)
        {
            _knockBack.StopKnockBackMovement();
            _isAlive = false;
            GameInput.Instance.DisableMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
           
    }

    //Метод вызывающий корутину для отчета времени
    private IEnumerator DamageRecoveryRoutine()
    {
            //Отчитываем количество фреймов равное полю восстановления для получения урона
            yield return new WaitForSeconds(_damageRecoveryTime);
            //Выставляем возможность получения урона
            _canTakeDamage = true;
    }

}

```
Пора присвоить ей анимацию в PlayerVisual

```

public class PlayerVisual : MonoBehaviour
{
    //Объявим переменную атаку магией
    [SerializeField] private WitchAttack _witchAttack;
    //Объявляем перменные аниматора и спрайтРендера
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    /*[SerializeField] Player Player;*/

    //Опишем текстовые константы для упрощенной работы с аниматором
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";
    private const string TAKE_DAMAGE = "TakeDamage";
    private const string MAGIC_ATTACK = "MagicAttack";

    private void Awake()
    {
        //Кешируем аниматор и спрайтрендер
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        //Подписываемся на события Смерти и Получения урона персонажа
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Player.Instance.OnTakeHit += Player_OnTakeHit;
        
    }

    //Событие получение урона
    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        //Аниматор устанавливает Триггер на получание урона
        animator.SetTrigger(TAKE_DAMAGE);

    }

    //Событие смерти
    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        //Выставляем статус смерти
        animator.SetBool(IS_DEAD, true);
    }

    private void Update()
    {
        //Проверяем бежит наш герой или нет и ставим соотвествующий статус
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());

        //Если наш персонаж живой
        if (Player.Instance.IsAlive())
        {
            //Следим за направлением мыши
            AdjustPlayerFacingDirection();
            //Подписываемся на событие магической атаки
            GameInput.Instance.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;
        }
    }

    //Событие магической атаки
    private void Player_OnPlayerMagicAttack(object sender, System.EventArgs e)
    {
        //Устанавливаем тригер анимамтора 
        animator.SetTrigger(MAGIC_ATTACK);
    }

    //Следим за нахождением мыши на экране и попорачиваем персонажа в ее сторону
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos=GameInput.Instance.GetMousePosition();
        Vector3 playerPos=Player.Instance.GetPlayerScreenPosiyion();

        if (mousePos.x < playerPos.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    public void triggerEndAttackAnimation()
    {
        //Метод вызываемый в классе Sword, для отключения колайдера
        _witchAttack.AttackCollaiderTurnOff();
    }

}
```
