using Assets.Scripts.gameEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Player;
using Assets.ServiceLocator;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

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
    public Inventory playerInventory;

    //Объявляем события Смерти и получения урона
    public event EventHandler OnPlayerDeath;
    public event EventHandler<DamageEventArgs> OnTakeHit;
    public event EventHandler OnPlayerUpdateCurrentExpirience;
    public event EventHandler OnPlayerUpdateCurrentMana;
    public event EventHandler OnPlayerUpdateCurrentHealth;

    //Объявляем переменные
    //Скорость, макс здоровье, время востановления для получения урона, место нахождения
    [SerializeField] private float _speed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxMana;
    [SerializeField] private float _maxExpirience;
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
    private float _currentMana;
    private float _currentExpirience;
    private bool _canTakeDamage;
    private bool _isAlive = true;

    private PlayerStatsUIManager _statsUIManager;

    //Переменная отвечающая за свет от персонажа
    private Light2D _playerLight;

    // Попробуем добавить ПАТТЕРН НАБЛЮДАТЕЛЬ
    private Subject _subject = new Subject();

    //Добавляем ПАТЕРН СЕРВИС ЛОКАТОР
    private IGameInput _gameInput;


    //[Inject]
    //public void Construct(IGameInput gameInput)
    //{
    //    _gameInput = gameInput;
    //    _gameInput.StartManager();
    //    _gameInput.OnPlayerAttack += Player_OnPlayerAttack;
    //    _gameInput.OnPlayerRangeAttack += Player_OnPlayerRangeAttack;
    //    _gameInput.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;
    //}

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
        //Может получать урон
        _canTakeDamage=true;
        _gameInput = ServiceLocator.GetService<IGameInput>();
        //Подписываемся на события атаки 
        _gameInput.OnPlayerAttack += Player_OnPlayerAttack;
        _gameInput.OnPlayerRangeAttack += Player_OnPlayerRangeAttack;
        _gameInput.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;

        SetPlayerCharacteristics();
        SetPlayerAchivements();
        SetPlayerActuallyStats();

        ////Устанавливаем текущее здоровье = максимальноиу
        _currentHealth = _maxHealth;
        _currentExpirience = 0;
        _currentMana = _maxMana;

        _statsUIManager = FindAnyObjectByType<PlayerStatsUIManager>();
        _statsUIManager.StartManager();
        _statsUIManager.StartPlayerStatsUIManager(_maxHealth, _maxMana);

        LightSetting(); //Вызываем метод для установки света у нашего персонажа


        // ПРисвоим инвентарю персонажа - инвентарь из наших данных 
        //playerInventory = GameManager.Instance.playerData.playerInventory;
        //SetPlayerInventory();

    }
    private void OnDisable()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        //Подписываемся на события атаки 
        _gameInput.OnPlayerAttack -= Player_OnPlayerAttack;
        _gameInput.OnPlayerRangeAttack -= Player_OnPlayerRangeAttack;
        _gameInput.OnPlayerMagicAttack -= Player_OnPlayerMagicAttack;
    }
    private void OnDestroy()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        //Подписываемся на события атаки 
        _gameInput.OnPlayerAttack -= Player_OnPlayerAttack;
        _gameInput.OnPlayerRangeAttack -= Player_OnPlayerRangeAttack;
        _gameInput.OnPlayerMagicAttack -= Player_OnPlayerMagicAttack;

    }
    private void Update()
    {
        //Отслеживание вектора персонажа
        //_inputVector = GameInput.Instance.GetMovementVector();
        _inputVector = _gameInput.GetMovementVector();
        // Debug.Log(GameManager.Instance.user.GetName());

        // Поделючим инвентарь нашего персонажа к данным об игроке
        GameManager.Instance.playerData.playerInventory = playerInventory;

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
        _currentMana -= 5;
        GetCurrentManaEvent();

    }
    private void Player_OnPlayerRangeAttack(object sender, EventArgs e)
    {
        //Вызываем метод в атаки в классе Актив Вепон
        ActiveWeapon.Instance.GetMagicalBall().Attack();
        _currentMana -= 2;
        GetCurrentManaEvent();
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
    public float GetCurrentMana() => _currentMana;
    public float GetCurrentExpirience()=>_currentExpirience;
    public Subject GetSubject() => _subject;
    public float GetMaxHealth() => _maxHealth;
    public float GetMaxMana() => _maxMana;
    public float GetMaxExpirience()=>_maxExpirience;
  
    public float SetCurrentHealth(float health)
    {
       return _currentHealth = health;
    }
    public float SetCurrentMana(float mana)
    {
        return _currentMana = mana;
    }
    public float SetCurrentExpirience(float exp)
    {
        return _currentExpirience = exp;
    }

    // Методы вызывающие события для обновления значений характеристик баров у перса
    public void GetCurrentHealthEvent()
    {
        OnPlayerUpdateCurrentHealth?.Invoke(this, EventArgs.Empty);
    }
    public void GetCurrentManaEvent()
    {
        OnPlayerUpdateCurrentMana?.Invoke(this, EventArgs.Empty);
    }
    public void GetCurrentExpirienceEvent()
    {
        OnPlayerUpdateCurrentExpirience?.Invoke(this, EventArgs.Empty);
    }

    //Получение позиции нашего персонажа относительно экрана
    public Vector3 GetPlayerScreenPosition()
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

            //VМетод вызываемый при обновлении здоровья перса
            GetCurrentHealthEvent();

            //Вызываем метод отскакивания при получении урона
            _knockBack.GetKnockBack(damageSourse);
            //Больше не можем получать урон
            _canTakeDamage = false;
           // OnTakeHit?.Invoke(this, EventArgs.Empty);

            // Вызываем событие о получении урона, передаем текущий уровень здоровья
            OnTakeHit?.Invoke(this, new DamageEventArgs { Damage = damage, CurrentHealth = _currentHealth });

            NotifyObservers("PlayerTakesDamage");

            //Начинаем отчет до следующей возможности получать урон
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log(_currentHealth);
        }
        //Отслеживае состояние смерти
        DetectDeath();
    }
    private void NotifyObservers(string message)
    {
        _subject.NotifyObservers(message);
    }

    private void SetPlayerCharacteristics()
    {
        playerStats = GameManager.Instance.playerData.playerStats;
    }

    // В данном методе будем устанавливать Актуальные характеристики нашего персонажа от его статистик
    private void SetPlayerActuallyStats()
    {
        _speed = playerStats.speed;
        _maxHealth = playerStats.maxHealth;
        _maxMana = playerStats.maxMana;
        _maxExpirience = playerStats.currentExperience;
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
        if (_currentHealth <= 0 && _isAlive)
        {
            _knockBack.StopKnockBackMovement();
            _isAlive = false;
            _gameInput.DisableMovement();
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
