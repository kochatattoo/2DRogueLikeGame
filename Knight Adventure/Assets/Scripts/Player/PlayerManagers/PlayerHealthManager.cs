using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
       //Обращаемся к переменой Изображения
    [SerializeField]private Image _healthBar;
    //Переменная количества здоровья
    private float _healthAmount;
    private float _maxHealth;
    private float _healthCurrent;
    private float _damage;

    private void Start()
    {
        //_maxHealth = Player.Instance.GetMaxHealth();
        //_healthAmount = Player.Instance.GetMaxHealth();
        Player.Instance.OnTakeHit += Player_OnTakeHit;
    }
    private void OnDisable()
    {
        Player.Instance.OnTakeHit -= Player_OnTakeHit;
    }

    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        _healthCurrent = Player.Instance.GetCurrentHealth();
        _damage = _healthAmount - _healthCurrent;
        TakeDamage(_damage);
        
    }

    //Метод отвечающий за получение урона
    public void TakeDamage(float damage)
    {
        //Переприсваиваем значение здоровья
        _healthAmount-=damage;
        //Выставляем соответствующий процент на шкале Filled
        _healthBar.fillAmount = _healthAmount/_maxHealth;
    }
    //Метод отвечающий за исцеление
    public void Heal(int healingAmount)
    {
        //Переприсваиваем значение здоровья
        _healthAmount+=healingAmount;
        //Не может превышать 100 и быть ниже 0
        _healthAmount=Mathf.Clamp(_healthAmount, 0, _maxHealth);

        //Выставляем соответствующий процент на шкале Filled
        _healthBar.fillAmount = _healthAmount / _maxHealth;
    }
    public float SetHealthBar(int health)
    {
        return _healthAmount = health;
    }
    public void StartPlayerHealthManager(float maxHealth)
    {
        //_maxHealth = Player.Instance.GetMaxHealth();
        //_healthAmount = Player.Instance.GetMaxHealth();

        _maxHealth = maxHealth;
        _healthAmount = maxHealth;
    }
}
