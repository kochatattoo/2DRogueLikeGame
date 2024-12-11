using Assets.Scripts.Player;
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

    //�������� ������ �� ��������� ��������� - �� �� ������ ������ � ��������� ������ ����������
    //����� �������� �� ���� �� ������ 
    public Inventory playerInventory;

    //��������� ������� ������ � ��������� �����
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnTakeHit;

    //��������� ����������
    //��������, ���� ��������, ����� ������������� ��� ��������� �����, ����� ����������
    [SerializeField] private float _speed;
    [SerializeField] private int _maxHealth;
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
    private bool _canTakeDamage;
    private bool _isAlive = true;

    //���������� ���������� �� �������
    private PlayerHealthManager _healthBar;

    //���������� ���������� �� ���� �� ���������
    private Light2D _playerLight;

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
        //������������� ������� �������� = �������������
         _currentHealth=_maxHealth;
        //����� �������� ����
        _canTakeDamage=true;
        //������������� �� ������� ����� 
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        GameInput.Instance.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;

        SetPlayerCharacteristics();
        SetPlayerAchivements();
        SetPlayerActuallyStats();
        //SetPlayerInventory();
        LightSetting(); //�������� ����� ��� ��������� ����� � ������ ���������
    }

  
    private void Update()
    {
        //������������ ������� ���������
        _inputVector = GameInput.Instance.GetMovementVector();
        // Debug.Log(GameManager.Instance.user.GetName());
    
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
    public float GetMaxHealth() => _maxHealth;
  
    public float SetCurrentHealth(float health)
    {
       return _currentHealth = health;
    }

    //��������� ������� ������ ��������� ������������ ������
    public Vector3 GetPlayerScreenPosiyion()
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
            //�������� ����� ������������ ��� ��������� �����
            _knockBack.GetKnockBack(damageSourse);
            //������ �� ����� �������� ����
            _canTakeDamage = false;
            OnTakeHit?.Invoke(this, EventArgs.Empty);
            //�������� ����� �� ��������� ����������� �������� ����
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log(_currentHealth);
        }
        //���������� ��������� ������
        DetectDeath();
    }
    private void SetPlayerCharacteristics()
    {
        playerStats = GameManager.Instance.playerData.playerStats;
    }

    // � ������ ������ ����� ������������� ���������� �������������� ������ ��������� �� ���� ���������
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

    //����� ������������� ��������� ������ �����
    private void DetectDeath()
    {
        //���� ������� �������� ����� 0, �� ������������� �����
        if (_currentHealth == 0)
        {
            _knockBack.StopKnockBackMovement();
            _isAlive = false;
            GameInput.Instance.DisableMovement();
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
