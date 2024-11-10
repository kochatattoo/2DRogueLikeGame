using System;
using UnityEngine;

//������������� ������� ��������� ���������������, ���� ���� ��� �� ����� � ����������
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent (typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    //��������� ������� ��������� ����� � ������
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    //��������� ���������� ���������� �� ������� ��������
    private int _currentHealth;

    //���������� ������� ���������
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        //�������� ���������������
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _polygonCollider2D.enabled = false;
        //�������� ������������
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.enabled = true;

        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        //��� ������ ����������� �������� ���� � ��������
        _currentHealth=_enemySO.enemyHealth;
    }

    //����� ��� ����������� ����������
    private void OnTriggerStay2D(Collider2D collision)
    {
       if (collision.transform.TryGetComponent(out Player player))
        {
            //�������� ����� ���������� ����� ������� ������
            player.TakeDamage(transform, _enemySO.EnemyDamageAmount);
            //PlayerHealthManager.Instance.TakeDamage(_enemySO.EnemyDamageAmount);
        }
    }

    //����� ���������� �� ��������� �����
    public void TakeDamage(int damage)
    {
        //��������������� �������� �������� ��������
        _currentHealth-=damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        //����������� ��������� ������
        DetectDeth();
    }

    //����� ����������� ���������������
    public void PolygonCollaiderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    //����� ���������� ���������������
    public void PolygonCollaiderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    //����� ������������� ������ �����
    private void DetectDeth()
    {
        //���� �������� ����� 0, �� ���������� ������
        if (_currentHealth <= 0) {
            //�������� ������� ������
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;

            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

}
