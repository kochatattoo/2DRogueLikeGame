using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//��������� ����� ����
public class Sword : MonoBehaviour
{
    //������� ���������� �����������
    [SerializeField] private int _damageAmount = 2;
    //�������� ������� "����� ����"
    public event EventHandler OnSwordSwing;
    //������ ���������� ������� �������� 2�
    private PolygonCollider2D _polygonCollider2D;

    //����� �����, ���������� ��� ������ ���������
    private void Awake()
    {
        //�������������� ���������������2� (��������)
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _polygonCollider2D.enabled = false;
    }

    //����� �����, ���������� ����� ������ ������� ������
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
        //�������� ������� "����� ����" � ��������� �������� ��������
        OnSwordSwing?.Invoke(this,EventArgs.Empty);
    }

    //�����, ��� �������� �� ��������� �� ��������, ������� ��������� � 2�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��������� ��� ������� �� �����
        //���������� � ������ �������� ���� EnemyEntuty ��-�� out
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
