using Assets.Scripts.gameEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Player;
using Assets.ServiceLocator;
using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[SelectionBase]
public class Player : MonoBehaviour
{
    //��������� ����� ����������� � ������ �� ���� ��������
    public static Player Instance {  get; private set; }

    //����� �������� ���������� ��� ��������� � ����������
    public PlayerData playerData;

    //��������� ������ �� ���������� ���������
    public PlayerStats playerStats;
    public PlayerAchievements playerAchievements;
    public Inventory playerInventory;
    public ActiveWeapon playerActiveWeapon;

    //��������� ������� ������ � ��������� �����
    public event EventHandler OnPlayerDeath;
    public event EventHandler<DamageEventArgs> OnTakeHit;
    public event EventHandler OnPlayerUpdateCurrentExpirience;
    public event EventHandler OnPlayerUpdateCurrentMana;
    public event EventHandler OnPlayerUpdateCurrentHealth;

    //��������� ����������
    //��������, ���� ��������, ����� ������������� ��� ��������� �����, ����� ����������
    [SerializeField] private float _speed { get; set; }
    [SerializeField] private float _maxHealth {  get; set; }
    [SerializeField] private float _maxMana { get; set; }
    [SerializeField] private float _maxExpirience { get; set; }
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    Vector2 _inputVector;

    //���������� RigidBody (������) � ����� ���������� �� ������������ ��� ��������� �����
    private Rigidbody2D _rb;
    private knockBack _knockBack;

    //����������� �������� ������������ � ������ ���� = ����
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;


    //������� ��������, �������� �� �������� ����, ������ �����
    private float _currentHealth { get; set; } 
    private float _currentMana { get; set; }
    private float _currentExpirience { get; set; }
    private bool _canTakeDamage {  get; set; } 
    private bool _isAlive = true;

    private Coroutine _regenCoroutine;
    private float _healthRegenRate = 2f; // ���������� ��������, ����������������� � �������
    private float manaRegenRate = 5f;   // ���������� ����, ����������������� � �������
    private float _regenInterval = 1f; // �������� �����������

    [SerializeField] private PlayerAnimationAttack _playerAnimationAttack;
    private Coroutine _skillsCoroutine; // TODO: ��������� �������� ������ ����� (��� �����������) 
    private float _timeInterval = 5f; // �������� �������� ����������� �� �������, ���� ���� ������� ����� ���� ����������� ����� ������
    private bool _canUseSkills { get; set; }

    private PlayerStatsUIManager _statsUIManager;

    //���������� ���������� �� ���� �� ���������
    private Light2D _playerLight;

    private CinemachineVirtualCamera _cineMachine;

    //��������� ������ ������ �������
    private IGameInput _gameInput;
    private ISaveManager _saveManager;
    private IAutarizationManager _autarizationManager;


    void Awake()
    {
        //�������������� ��������
       Instance = this; // TODO - ����� ��� ����� ���������� �� ��������� � Player � ����������� ��� ������ �� ��������� (���� ��� ���������� PlaterData)
        //�������� ����������
        _rb= GetComponent<Rigidbody2D>();
        _knockBack=GetComponent<knockBack>();
    }
    private void Start()
    {
        //����� �������� ����
        _canTakeDamage= true;
        _canUseSkills= true;
        InitializeServices();
        SubscribeGameInputEvent();
        InitializeGameComponents();

        LoadPlayerData();
        SetPlayer();
     
        _statsUIManager = FindAnyObjectByType<PlayerStatsUIManager>();
        _statsUIManager.StartManager();
        _statsUIManager.StartPlayerStatsUIManager(_maxHealth, _maxMana);

        // ��������� ��������, ����� ������ ��������������
        _regenCoroutine = StartCoroutine(RegenCoroutine());

        LightSetting(); //�������� ����� ��� ��������� ����� � ������ ���������

        _cineMachine = FindObjectOfType<CinemachineVirtualCamera>();
        if (_cineMachine != null)
        {
            _cineMachine.Follow = this.transform;
        }
        else
        {
            var notifiactionManager = ServiceLocator.GetService<INotificationManager>();
            notifiactionManager.OpenNotificationWindow("Error", "Cinemachine camera hasn.t founded in the Scene");
            Debug.Log("Cinemachine camera hasn.t founded in the Scene");
        }

    }
    private void InitializeServices()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        _saveManager = ServiceLocator.GetService<ISaveManager>();
        _autarizationManager = ServiceLocator.GetService<IAutarizationManager>();
    }
    private void SubscribeGameInputEvent()
    {
        //������������� �� ������� ����� 
        _gameInput.OnPlayerAttack += Player_OnPlayerAttack;
        _gameInput.OnPlayerRangeAttack += Player_OnPlayerRangeAttack;
        _gameInput.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;
    }
    private void InitializeGameComponents()
    {
        _playerAnimationAttack.StartScript();
    }
    private void LoadPlayerData()
    {
        playerData = _autarizationManager.GetPlayerData();
    }
    private void OnDisable()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        //������������� �� ������� ����� 
        _gameInput.OnPlayerAttack -= Player_OnPlayerAttack;
        _gameInput.OnPlayerRangeAttack -= Player_OnPlayerRangeAttack;
        _gameInput.OnPlayerMagicAttack -= Player_OnPlayerMagicAttack;
    }
    private void OnDestroy()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        //������������� �� ������� ����� 
        _gameInput.OnPlayerAttack -= Player_OnPlayerAttack;
        _gameInput.OnPlayerRangeAttack -= Player_OnPlayerRangeAttack;
        _gameInput.OnPlayerMagicAttack -= Player_OnPlayerMagicAttack;

    }
    private void Update()
    {
        _inputVector = _gameInput.GetMovementVector();
        // ��������� ��������� ������ ��������� � ������ �� ������
    }

    void FixedUpdate()
    {
        //��������� ���������� �� �� � ��������� ������
        if (_knockBack.IsGettingKnockedBack)
            return;
        //����� ����������� ������ ����
        HandleMovement();
    }

    //������� ����� 
    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        //�������� ����� � ����� � ������ ����� �����
        playerActiveWeapon.SwordWeapon.Attack();
    }
    //������� ���������� �����
    private void Player_OnPlayerMagicAttack(object sender, EventArgs e)
    {
        if (_canUseSkills)
        {
            //�������� ����� � ����� � ������ ����� �����
            playerActiveWeapon.MagicAttack.Attack();
            _currentMana -= 5;
            GetCurrentManaEvent();
            _canUseSkills = false; // TODO: ���� ��������� ��������, ��� �� � �� ������ ���� �������������
            _skillsCoroutine = StartCoroutine(SkillsCoroutine()); // ��� ��� ������ ���������� ��������
        }
        else
        {
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.PlayNotificationAudio("Lock");
        }
    }
    private void Player_OnPlayerRangeAttack(object sender, EventArgs e)
    {
        //�������� ����� � ����� � ������ ����� �����
        playerActiveWeapon.MagicalBall.Attack();
        _currentMana -= 2;
        GetCurrentManaEvent();
    }

    //������������ ������� ���� ���������
    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * _speed * Time.fixedDeltaTime);

        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed){
            _isRunning = true;
        } else{
            _isRunning= false;
        }
    }

    // ��������� ����������� ������ ������������ � ��������������� �������� ����� ������
    // �� �������� ������ 
    public bool IsRunning
    {
        get { return _isRunning; }
        set { _isRunning = value; }
    }
    public bool IsAlive
    {
        get { return _isAlive; }
        set { _isAlive = value; }
    }
    public float CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
    public float CurrentMana
    {
        get { return _currentMana; }
        set { _currentMana = value; }
    }
    public float CurrentExpirience
    {
        get { return _currentExpirience; }
        set { _currentExpirience = value; }
    }
    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    public float MaxMana
    {
        get { return _maxMana; }
        set { _maxMana = value; }
    }
    public float MaxExpirience
    {
        get { return _maxExpirience; }
        set { _maxExpirience = value; }
    }

    public bool CanUseSkills
    {
        get { return _canUseSkills; }
        set { _canUseSkills = value; }
    }
    // ������ ���������� ������� ��� ���������� �������� ������������� ����� � �����
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

    //��������� ������� ������ ��������� ������������ ������
    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition=Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public Vector3 GetPlayerPosition()
    {
        return gameObject.transform.position;
    }
    public void SetPlayerPosition(Vector3 playerPosition)
    {
        gameObject.transform.position = playerPosition;
    }
    //����� ���������� ��� ��������� ����� ������� ������
    public void TakeDamage(Transform damageSourse, int damage)
    {
        //���� �� ����� �������� ����
        if (_canTakeDamage&&_isAlive)
        {
            //����������� ������� �������� � ��������� 0 � ���� ��������
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);

            //V����� ���������� ��� ���������� �������� �����
            GetCurrentHealthEvent();

            //�������� ����� ������������ ��� ��������� �����
            _knockBack.GetKnockBack(damageSourse);
            //������ �� ����� �������� ����
            _canTakeDamage = false;
           // OnTakeHit?.Invoke(this, EventArgs.Empty);

            // �������� ������� � ��������� �����, �������� ������� ������� ��������
            OnTakeHit?.Invoke(this, new DamageEventArgs { Damage = damage, CurrentHealth = _currentHealth });

            //�������� ����� �� ��������� ����������� �������� ����
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log(_currentHealth);
        }
        //���������� ��������� ������
        DetectDeath();
    }

    private void SetPlayerCharacteristics()
    {
        playerStats = playerData.playerStats;
    }
    // � ������ ������ ����� ������������� ���������� �������������� ������ ��������� �� ��� ���������
    private void SetPlayerActuallyStats()
    {
        _speed = playerStats.speed;
        _maxHealth = playerStats.maxHealth;
        _maxMana = playerStats.maxMana;
        _maxExpirience = playerStats.currentExperience;
    }
    private void SetPlayerCurrentStats()
    {
        ////������������� ������� �������� = �������������
        _currentHealth = _maxHealth;
        _currentExpirience = 0; // TODO: ��������� ������� ��������� ���������
        _currentMana = _maxMana;
    }
    private void SetPlayerAchivements()
    {
        playerAchievements = playerData.playerAchievements;
    }
    private void SetPlayerInventory()
    {
        // TODO: ��� ���������� ������ ��� �������� ��������� �� PlayerData � Player, ������ ��������� ������
    }
    private void SetPlayer()
    {
        SetPlayerCharacteristics();
        SetPlayerAchivements();
        SetPlayerActuallyStats();
        SetPlayerCurrentStats();
        SetPlayerInventory();
    }
    //����� ������������� ��������� ������ �����
    private void DetectDeath()
    {
        //���� ������� �������� ����� 0, �� ������������� �����
        if (_currentHealth <= 0 && _isAlive)
        {
            _knockBack.StopKnockBackMovement();

            if (_regenCoroutine != null)
            {
                StopCoroutine(_regenCoroutine);
            }
            _isAlive = false;
            _gameInput.DisableMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
           
    }

    //����� ���������� �������� ��� ������ �������
    private IEnumerator DamageRecoveryRoutine()
    {
            //���������� ���������� ������� ������ ���� �������������� ��� ��������� �����
            yield return new WaitForSeconds(_damageRecoveryTime);
            //���������� ����������� ��������� �����
            _canTakeDamage = true;
    }
    private IEnumerator RegenCoroutine()
    {
        while (true)
        {
            // ��������������� ��������
            if (_currentHealth < _maxHealth)
            {
                _currentHealth += _healthRegenRate * _regenInterval;
                _currentHealth = Mathf.Min(_currentHealth, _maxHealth); // ���������, ��� �������� �� ��������� ��������\
                GetCurrentHealthEvent();
            }

            // ��������������� ����
            if (_currentMana < _maxMana)
            {
                _currentMana += manaRegenRate * _regenInterval;
                _currentMana = Mathf.Min(_currentMana, _maxMana); // ���������, ��� ���� �� ��������� ��������
                GetCurrentManaEvent();
            }

            yield return new WaitForSeconds(_regenInterval); // ���� ��������� ��������
        }

    }
    private IEnumerator SkillsCoroutine()
    {
        // TODO: ��� ��� ��� ����� ������ ������ ��� �������� 
        yield return new WaitForSeconds(_timeInterval); // ���� ��������� ��������
        _canUseSkills = true;
    }

    private void LightSetting()
    {
        // �������� ��������� ����� ��� ������
        GameObject lightObject = new GameObject("PlayerLight");
        _playerLight = lightObject.AddComponent<Light2D>();
        _playerLight.lightType = Light2D.LightType.Point;
        _playerLight.intensity = 2f; // �������
        _playerLight.pointLightInnerRadius = 1f; // ���������� ������
        _playerLight.pointLightOuterRadius = 3f; // ������� ������
        _playerLight.color=Color.cyan; // ����� �����

        // ��������� ����� ��� ��������� ������� ���������
        lightObject.transform.parent = transform; // ������������� ������ ����� ��� �������� � ���������
        lightObject.transform.localPosition = new Vector3(0, 0.5f, 0); // ������� ������������ ���������
    }

}
