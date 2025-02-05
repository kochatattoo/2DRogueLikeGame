using Assets.Scripts.gameEventArgs;
using Assets.Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : MonoBehaviour, IObserver
{
    // ���������� � ���������� ����������� � ������
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _manaBar;
    [SerializeField] private Image _experienceBar;
    [SerializeField] private TextMeshProUGUI _experienceText;

    // ���������� ���������� ��������
    private float _healthAmount;
    private float _maxHealth;

    // ���������� ���������� ����
    private float _manaAmount;
    private float _maxMana;

    // ���������� �����
    private int _experience; // ������� ����
    private int _level; // ������� ������
    private int _experienceToNextLevel; // ���� �� ���������� ������

    private void Start()
    {
        //Subject subject = Player.Instance.GetSubject();
        //if (subject != null)
        //{
        //    subject.RegisterObserver(this);
        //}


        //Player.Instance.OnTakeHit += Player_OnTakeHit;
        //Player.Instance.OnPlayerUpdateCurrentExpirience += Player_OnPlayerUpdateCurrentExpirience;
        //Player.Instance.OnPlayerUpdateCurrentHealth += Player_OnPlayerUpdateCurrentHealth;
        //Player.Instance.OnPlayerUpdateCurrentMana += Player_OnPlayerUpdateCurrentMana;

        
        // ����� ����� ����������� �� ������� ���� � �����, ���� ��� ����
    }
    public void StartManager()
    {
        Player.Instance.OnTakeHit += Player_OnTakeHit;
        Player.Instance.OnPlayerUpdateCurrentExpirience += Player_OnPlayerUpdateCurrentExpirience;
        Player.Instance.OnPlayerUpdateCurrentHealth += Player_OnPlayerUpdateCurrentHealth;
        Player.Instance.OnPlayerUpdateCurrentMana += Player_OnPlayerUpdateCurrentMana;
    }
  
    public void OnNotify(string message)
    {
        if (message == "PlayerTakesDamage")
        {
            // ������� �� ����������� � ��������� �����
            Debug.Log("Player takeDAMAGE triggered!");
            // ������ ������ ������
        }
    }
    private void OnDisable()
    {
        //Player.Instance.OnTakeHit -= Player_OnTakeHit;
        //Player.Instance.OnPlayerUpdateCurrentExpirience -= Player_OnPlayerUpdateCurrentExpirience;
        //Player.Instance.OnPlayerUpdateCurrentHealth -= Player_OnPlayerUpdateCurrentHealth;
        //Player.Instance.OnPlayerUpdateCurrentMana -= Player_OnPlayerUpdateCurrentMana;
    }
    public void OnDestroyManager()
    {
        Player.Instance.OnTakeHit -= Player_OnTakeHit;
        Player.Instance.OnPlayerUpdateCurrentExpirience -= Player_OnPlayerUpdateCurrentExpirience;
        Player.Instance.OnPlayerUpdateCurrentHealth -= Player_OnPlayerUpdateCurrentHealth;
        Player.Instance.OnPlayerUpdateCurrentMana -= Player_OnPlayerUpdateCurrentMana;
    }

    private void Player_OnTakeHit(object sender, DamageEventArgs e)
    {
        _healthAmount=e.CurrentHealth; // �������� ����� �������
        _healthAmount = Player.Instance.GetCurrentHealth(); // ���������� ����� ��������
        TakeDamage();
    }
    private void Player_OnPlayerUpdateCurrentMana(object sender, System.EventArgs e)
    {
        _manaAmount = Player.Instance.GetCurrentMana();
        ChangeMana();
    }

    private void Player_OnPlayerUpdateCurrentHealth(object sender, System.EventArgs e)
    {
        _healthAmount = Player.Instance.GetCurrentHealth();
        TakeDamage();
    }

    private void Player_OnPlayerUpdateCurrentExpirience(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }
    // ����� ���������� �� ��������� �����
    public void TakeDamage()
    {
        // _healthAmount -= damage;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, _maxHealth);
        _healthBar.fillAmount = _healthAmount / _maxHealth;
    }

    // ����� ���������� �� ���������
    public void Heal(float healingAmount)
    {
        _healthAmount += healingAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, _maxHealth);
        _healthBar.fillAmount = _healthAmount / _maxHealth;
    }

    // ����� ��� ��������� ����
    public void ChangeMana()
    {
       // _manaAmount -= amount;
        _manaAmount = Mathf.Clamp(_manaAmount, 0, _maxMana);
        _manaBar.fillAmount = _manaAmount / _maxMana;
    }

    // ����� ��� ��������� �������� �����
    public void SetExperience(int experience)
    {
        _experience = experience;
        UpdateExperienceUI();
    }

    // ����� ��� ��������� �����
    public void AddExperience(int amount)
    {
        _experience += amount;
        UpdateExperienceUI();
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (_experience >= _experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _level++;
        _experience -= _experienceToNextLevel;
        _experienceToNextLevel = CalculateExperienceToNextLevel();
        // ����� ����� �������� �������������� ������� ������
    }

    private int CalculateExperienceToNextLevel()
    {
        // ������ ������� ����� �� ���������� ������ (����� ������������)
        return _level * 100; // ����������� ����������� ���� � ������ �������
    }

    public void StartPlayerStatsUIManager(float maxHealth, float maxMana)
    {
        _maxHealth = maxHealth;
        _healthAmount = maxHealth;

        _maxMana = maxMana;
        _manaAmount = maxMana;

        // ������������� UI
        _healthBar.fillAmount = 1f; // ������������� ��������
        _manaBar.fillAmount = 1f; // ������������� ����
        UpdateExperienceUI();
    }

    private void UpdateExperienceUI()
    {
        _experienceText.text = $"Experience: {_experience} / {_experienceToNextLevel}";
    }
  
}