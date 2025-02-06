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
    //��������� ����� ����������� � ������ �� ���� ��������
    public static Player Instance {  get; private set; }

    //����� �������� ���������� ��� ��������� � ����������
    public PlayerData playerData;

    //��������� ������ �� ���������� ���������
    public PlayerStats playerStats;
    public PlayerAchievements playerAchievements;
    public Inventory playerInventory;

    //��������� ������� ������ � ��������� �����
    public event EventHandler OnPlayerDeath;
    public event EventHandler<DamageEventArgs> OnTakeHit;
    public event EventHandler OnPlayerUpdateCurrentExpirience;
    public event EventHandler OnPlayerUpdateCurrentMana;
    public event EventHandler OnPlayerUpdateCurrentHealth;

    //��������� ����������
    //��������, ���� ��������, ����� ������������� ��� ��������� �����, ����� ����������
    [SerializeField] private float _speed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxMana;
    [SerializeField] private float _maxExpirience;
    [SerializeField] private float _damageRecoveryTime = 0.5f;
    Vector2 _inputVector;

    //���������� RigidBody (������) � ����� ���������� �� ������������ ��� ��������� �����
    private Rigidbody2D _rb;
    private knockBack _knockBack;

    //����������� �������� ������������ � ������ ���� = ����
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;


    //������� ��������, �������� �� �������� ����, ������ �����
    private float _currentHealth;
    private float _currentMana;
    private float _currentExpirience;
    private bool _canTakeDamage;
    private bool _isAlive = true;

    private PlayerStatsUIManager _statsUIManager;

    //���������� ���������� �� ���� �� ���������
    private Light2D _playerLight;

    // ��������� �������� ������� �����������
    private Subject _subject = new Subject();

    //��������� ������ ������ �������
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
        //�������������� ��������
       Instance = this;
        //�������� ����������
        _rb= GetComponent<Rigidbody2D>();
        _knockBack=GetComponent<knockBack>();
    }

    private void Start()
    {
        //����� �������� ����
        _canTakeDamage=true;
        _gameInput = ServiceLocator.GetService<IGameInput>();
        //������������� �� ������� ����� 
        _gameInput.OnPlayerAttack += Player_OnPlayerAttack;
        _gameInput.OnPlayerRangeAttack += Player_OnPlayerRangeAttack;
        _gameInput.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;

        SetPlayerCharacteristics();
        SetPlayerAchivements();
        SetPlayerActuallyStats();

        ////������������� ������� �������� = �������������
        _currentHealth = _maxHealth;
        _currentExpirience = 0;
        _currentMana = _maxMana;

        _statsUIManager = FindAnyObjectByType<PlayerStatsUIManager>();
        _statsUIManager.StartManager();
        _statsUIManager.StartPlayerStatsUIManager(_maxHealth, _maxMana);

        LightSetting(); //�������� ����� ��� ��������� ����� � ������ ���������


        // �������� ��������� ��������� - ��������� �� ����� ������ 
        //playerInventory = GameManager.Instance.playerData.playerInventory;
        //SetPlayerInventory();

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
        //������������ ������� ���������
        //_inputVector = GameInput.Instance.GetMovementVector();
        _inputVector = _gameInput.GetMovementVector();
        // Debug.Log(GameManager.Instance.user.GetName());

        // ��������� ��������� ������ ��������� � ������ �� ������
        GameManager.Instance.playerData.playerInventory = playerInventory;

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
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
 
    }
    //������� ���������� �����
    private void Player_OnPlayerMagicAttack(object sender, EventArgs e)
    {
        //�������� ����� � ����� � ������ ����� �����
        ActiveWeapon.Instance.GetMagicWeapon().Attack();
        _currentMana -= 5;
        GetCurrentManaEvent();

    }
    private void Player_OnPlayerRangeAttack(object sender, EventArgs e)
    {
        //�������� ����� � ����� � ������ ����� �����
        ActiveWeapon.Instance.GetMagicalBall().Attack();
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

    //���������� ������ ������ ��������� � ��������� �������� ����������
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

            NotifyObservers("PlayerTakesDamage");

            //�������� ����� �� ��������� ����������� �������� ����
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log(_currentHealth);
        }
        //���������� ��������� ������
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

    // � ������ ������ ����� ������������� ���������� �������������� ������ ��������� �� ��� ���������
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

    //����� ������������� ��������� ������ �����
    private void DetectDeath()
    {
        //���� ������� �������� ����� 0, �� ������������� �����
        if (_currentHealth <= 0 && _isAlive)
        {
            _knockBack.StopKnockBackMovement();
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
