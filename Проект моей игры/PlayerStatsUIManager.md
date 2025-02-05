Давайте изменим ваш класс `PlayerHealthManager`, чтобы он также управлял отображением значений здоровья, маны и опыта, а также переименуем его в более подходящее название, например, `PlayerStatsUIManager`. Мы будем использовать Unity UI для отображения этих значений через соответствующие `Image` и `Text` компоненты.

### Шаги по изменениям

1. **Переименование класса**: Переименуем класс в `PlayerStatsUIManager`.
2. **Добавление новых переменных**: Добавим переменные для хранения значений маны и опыта, а также соответствующие UI элементы (например, `Image` для маны и `Text` для опыта).
3. **Обновление UI**: Внесем изменения в методы для обновления UI на основе значений здоровья, маны и опыта.

### Обновленный код класса `PlayerStatsUIManager`

Вот обновленный код с указанными изменениями:
```
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : MonoBehaviour
{
    // Обращаемся к переменным Изображения и Текста
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _manaBar;
    [SerializeField] private Text _experienceText;

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
        Player.Instance.OnTakeHit += Player_OnTakeHit;
        // Здесь можно подписаться на события маны и опыта, если они есть
    }

    private void OnDisable()
    {
        Player.Instance.OnTakeHit -= Player_OnTakeHit;
    }

    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        _healthAmount = Player.Instance.GetCurrentHealth();
        TakeDamage(Player.Instance.GetMaxHealth() - _healthAmount);
    }

    // Метод отвечающий за получение урона
    public void TakeDamage(float damage)
    {
        _healthAmount -= damage;
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
    public void ChangeMana(float amount)
    {
        _manaAmount += amount;
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
```
### Объяснение изменений

1. **Переименование класса**: Теперь класс называется `PlayerStatsUIManager`, что отражает его новую ответственность за управление UI, связанным со статистикой игрока.
    
2. **Добавление переменных маны и опыта**:
    
    - Включены переменные для хранения текущей и максимальной маны, а также текущего опыта и уровня.
3. **Добавление методов для обновления маны и опыта**:
    
    - Методы `ChangeMana` и `SetExperience` для управления маной и опытом, а также метод `AddExperience`, который увеличивает текущий опыт и проверяет уровень.
4. **Обновление UI**: Логика обновления заполнения полосы здоровья, маны и текста опыта была реализована через методы, которые настраивают значения в UI.
    

### Заключение

С помощью этих изменений ваш класс теперь отвечает за визуальное отображение здоровья, маны и опыта вашего персонажа. Вы можете подключать события, связанные с изменением маны и опыта, чтобы обеспечить более сложную логику в проекте.

## 19.12.24 ВИД СКРИПТА ПРИ ДЕБАГИНГЕ
```
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : MonoBehaviour
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
        Player.Instance.OnTakeHit += Player_OnTakeHit;
        Player.Instance.OnPlayerUpdateCurrentExpirience += Player_OnPlayerUpdateCurrentExpirience;
        Player.Instance.OnPlayerUpdateCurrentHealth += Player_OnPlayerUpdateCurrentHealth;
        Player.Instance.OnPlayerUpdateCurrentMana += Player_OnPlayerUpdateCurrentMana;
        // Здесь можно подписаться на события маны и опыта, если они есть
    }

    private void OnDisable()
    {
        Player.Instance.OnTakeHit -= Player_OnTakeHit;
        Player.Instance.OnPlayerUpdateCurrentExpirience -= Player_OnPlayerUpdateCurrentExpirience;
        Player.Instance.OnPlayerUpdateCurrentHealth -= Player_OnPlayerUpdateCurrentHealth;
        Player.Instance.OnPlayerUpdateCurrentMana -= Player_OnPlayerUpdateCurrentMana;
    }

    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        _healthAmount = Player.Instance.GetCurrentHealth();
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
```