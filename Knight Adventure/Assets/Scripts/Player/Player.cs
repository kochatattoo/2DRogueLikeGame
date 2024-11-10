using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnTakeHit;

    [SerializeField] private float _speed = 15.0f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;
    Vector2 _inputVector;

    private Rigidbody2D _rb;
    private knockBack _knockBack;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive = true;

    private PlayerHealthManager _healthBar;

    void Awake()
    {
       Instance = this;
        _rb= GetComponent<Rigidbody2D>();
        _knockBack=GetComponent<knockBack>();
       
    }

    private void Start()
    {
        _currentHealth=_maxHealth;
        _canTakeDamage=true;
        GameInput.Instance._OnPlayerAttack += Player_OnPlayerAttack;
    }

    private void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();
        // Debug.Log(GameManager.Instance.user.GetName());
    
    }

    void FixedUpdate()
    {
        //Проверяем находимлся ли мы в состоянии отлета
        if (_knockBack.IsGettingKnockedBack)
            return;

        HandleMovement();
    }
    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
        
    }

    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * _speed * Time.fixedDeltaTime);

        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed){
            _isRunning = true;
        } else{
            _isRunning= false;
        }
    }

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
