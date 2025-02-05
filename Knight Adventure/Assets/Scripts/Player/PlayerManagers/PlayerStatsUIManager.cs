using Assets.Scripts.gameEventArgs;
using Assets.Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : MonoBehaviour, IObserver
{
    // Обращаемся к переменным Изображения и Текста
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _manaBar;
    [SerializeField] private Image _experienceBar;
    [SerializeField] private TextMeshProUGUI _experienceText;

    // Переменная количества здоровья
    private float _healthAmount;
    private float _maxHealth;

    // Переменная количества маны
    private float _manaAmount;
    private float _maxMana;

    // Переменная опыта
    private int _experience; // Текущий опыт
    private int _level; // Уровень игрока
    private int _experienceToNextLevel; // Опыт до следующего уровня

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

        
        // Здесь можно подписаться на события маны и опыта, если они есть
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
            // Реакция на уведомление о получении урона
            Debug.Log("Player takeDAMAGE triggered!");
            // Логика тряски камеры
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
        _healthAmount=e.CurrentHealth; // Передача через события
        _healthAmount = Player.Instance.GetCurrentHealth(); // Присвоение через синглтон
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
    // Метод отвечающий за получение урона
    public void TakeDamage()
    {
        // _healthAmount -= damage;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, _maxHealth);
        _healthBar.fillAmount = _healthAmount / _maxHealth;
    }

    // Метод отвечающий за исцеление
    public void Heal(float healingAmount)
    {
        _healthAmount += healingAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, _maxHealth);
        _healthBar.fillAmount = _healthAmount / _maxHealth;
    }

    // Метод для изменения маны
    public void ChangeMana()
    {
       // _manaAmount -= amount;
        _manaAmount = Mathf.Clamp(_manaAmount, 0, _maxMana);
        _manaBar.fillAmount = _manaAmount / _maxMana;
    }

    // Метод для установки текущего опыта
    public void SetExperience(int experience)
    {
        _experience = experience;
        UpdateExperienceUI();
    }

    // Метод для получения опыта
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
        // Здесь можно добавить дополнительные эффекты уровня
    }

    private int CalculateExperienceToNextLevel()
    {
        // Пример расчета опыта до следующего уровня (можно адаптировать)
        return _level * 100; // Увеличиваем необходимый опыт с каждым уровнем
    }

    public void StartPlayerStatsUIManager(float maxHealth, float maxMana)
    {
        _maxHealth = maxHealth;
        _healthAmount = maxHealth;

        _maxMana = maxMana;
        _manaAmount = maxMana;

        // Инициализация UI
        _healthBar.fillAmount = 1f; // Инициализация здоровья
        _manaBar.fillAmount = 1f; // Инициализация маны
        UpdateExperienceUI();
    }

    private void UpdateExperienceUI()
    {
        _experienceText.text = $"Experience: {_experience} / {_experienceToNextLevel}";
    }
  
}