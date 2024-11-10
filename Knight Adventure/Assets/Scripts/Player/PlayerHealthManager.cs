using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    //public static PlayerHealthManager Instance {  get; private set; }

    //���������� � ��������� �����������
    [SerializeField]private Image _healthBar;
    //���������� ���������� ��������
    private float _healthAmount;
    private float _healthCurrent;
    private float _damage;

    //private void Awake()
    //{
    //    Instance = this;
    //}

    private void Start()
    {
        _healthAmount = Player.Instance.GetMaxHealth();
        Player.Instance.OnTakeHit += Player_OnTakeHit;
    }

    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        _healthCurrent = Player.Instance.GetCurrentHealth();
        _damage = _healthAmount - _healthCurrent;
        TakeDamage(_damage);
        
    }

    //����� ���������� �� ��������� �����
    public void TakeDamage(float damage)
    {
        //��������������� �������� ��������
        _healthAmount-=damage;
        //���������� ��������������� ������� �� ����� Filled
        _healthBar.fillAmount = _healthAmount/10f;
    }
    //����� ���������� �� ���������
    public void Heal(int healingAmount)
    {
        //��������������� �������� ��������
        _healthAmount+=healingAmount;
        //�� ����� ��������� 100 � ���� ���� 0
        _healthAmount=Mathf.Clamp(_healthAmount, 0, Player.Instance.GetMaxHealth());

        //���������� ��������������� ������� �� ����� Filled
        _healthBar.fillAmount = _healthAmount / 10f;
    }
    public float SetHealthBar(int health)
    {
        return _healthAmount = health;
    }
}
