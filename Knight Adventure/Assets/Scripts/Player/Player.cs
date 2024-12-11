using Assets.Scripts.Player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[SelectionBase]
public class Player : MonoBehaviour
{
    //Объявляем класс статическим и делаем из него СинглТон
    public static Player Instance {  get; private set; }

    //Класс хранящий информацию для загрузыки и сохранения
    public PlayerData playerData;

    //Объявляем ссылку на статистику персонажа
    public PlayerStats playerStats;
    public PlayerAchievements playerAchievements;

    //Объявляю ссылку на инвентарь персонажа - но на данный момент в инвентарь доступ происходит
    //Через ссылание на свой же объект 
    public Inventory playerInventory;

    //Объявляем события Смерти и получения урона
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnTakeHit;

    //Объявляем переменные
    //Скорость, макс здоровье, время востановления для получения урона, место нахождения
    [SerializeField] private float _speed;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _damageRecoveryTime = 0.5f;
    Vector2 _inputVector;

    //Переменные RigidBody (физика) и класс отвечающий за отталкивание при получении урона
    private Rigidbody2D _rb;
    private knockBack _knockBack;

    //Минимальная скорость передвижения и статус бега = фолс
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;


    //текущее здоровье, возможно ли получать урон, статус жизни
    private float _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive = true;

    //Переменная отвечающая за хэлфбар
    private PlayerHealthManager _healthBar;

    //Переменная отвечающая за свет от персонажа
    private Light2D _playerLight;

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

        SetPlayerCharacteristics();
        SetPlayerAchivements();
        SetPlayerActuallyStats();
        //SetPlayerInventory();
        LightSetting(); //Вызываем метод для установки света у нашего персонажа
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
    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => _maxHealth;
  
    public float SetCurrentHealth(float health)
    {
       return _currentHealth = health;
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
    private void SetPlayerCharacteristics()
    {
        playerStats = GameManager.Instance.playerData.playerStats;
    }

    // В данном методе будем устанавливать Актуальные характеристики нашего персонажа от егго статистик
    private void SetPlayerActuallyStats()
    {
        _speed = playerStats.speed;
        _maxHealth = playerStats.maxHealth;
    }
    private void SetPlayerAchivements()
    {
        playerAchievements = GameManager.Instance.playerData.playerAchievements;
    }

    //private void SetPlayerInventory()
    //{
    //    playerInventory =GameManager.Instance.playerData.playerInventory;
    //}

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

    private void LightSetting()
    {
        // Создание источника света для игрока
        GameObject lightObject = new GameObject("PlayerLight");
        _playerLight = lightObject.AddComponent<Light2D>();
        _playerLight.lightType = Light2D.LightType.Point;
        _playerLight.intensity = 2f; // Яркость
        _playerLight.pointLightInnerRadius = 1f; // Внутренний радиус
        _playerLight.pointLightOuterRadius = 3f; // Внешний радиус
        _playerLight.color=Color.cyan; // Цвето света

        // Установка света как дочернего объекта персонажа
        lightObject.transform.parent = transform; // Устанавливаем объект света как дочерний к персонажу
        lightObject.transform.localPosition = new Vector3(0, 0.5f, 0); // Позиция относительно персонажа
    }
}
