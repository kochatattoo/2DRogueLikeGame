using UnityEngine;

public class PlayerAnimationAttack : MonoBehaviour
{
    //������� ���������� ���������� ����� 
    [SerializeField] private int _damageAmount = 5;
    //������ ���������� ������� ��������� 2�
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        //�������������� ��������������� (��������)
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _polygonCollider2D.enabled = false;
    }

    private void Start()
    {
        //��������� ��������������� � ����� ������
        AttackCollaiderTurnOff();
    }

    //�����, ���������� �����
    public void Attack()
    {
        //�������� ���������������2� ��� �����
        AttackCollaiderOffOn();
     
    }

    //�����, ��� �������� �� ��c����� �� ��������, ������� ��������� � 2�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��������� ��� ������� �� �����
        //���������� � ������ �������� ���� EnemyEntity ��-�� out
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            //�������� ����� �������������� � enemyEntity 
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    //�����, ����������� ���������������2�
    public void AttackCollaiderTurnOff()
    {
        //��������� ���������������2�
        _polygonCollider2D.enabled = false;
    }
    //�����, ���������� ���������������2�
    private void AttackCollaiderTurnOn()
    {
        //�������� ���������������2�
        _polygonCollider2D.enabled = true;
    }

    //����� ��� ����, ��� �� ��� ������� ������� � ������
    //������� ��������������� ���� �� ����������, ���� ���� 
    //�������� �� ������ ���������� � �������� ���������
    private void AttackCollaiderOffOn()
    {
        AttackCollaiderTurnOff();
        AttackCollaiderTurnOn();
    }
}
