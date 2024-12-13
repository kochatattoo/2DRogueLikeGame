using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicalBall : MonoBehaviour
{
    [SerializeField] private GameObject magicalBallPrefab;

    public event EventHandler OnMagicalBallCast;

    public float speed = 30f; // �������� ������ ����
    public int damage = 2; // ����, ������� ����� ����������

    private CircleCollider2D _circleCollider2D;
   
    private Vector2 direction; // ����������� �������� ����

    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
       // _circleCollider2D.enabled = false;
    }

    public void Initialize(Vector2 dir)
    {
        direction = dir;
    }
    private void Update()
    {
        // ���� ����������� �� � ������� ���������, ���������� ���
        if (direction != Vector2.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime); // ����������� ����
        }
    }
    public void Attack()
    {
        // ����� ������� ��� �� ��������� �������� ���� ������� ������, �� ����� �������� ����� ������ �� � ���
        // � ���� ��������� Vector2 playerPosition � ���������� ������ ���������� ��� �������� �������
        // � � ������ ����� � Player ��  ������� ��� ��������� (vector2)transform.position

        if (magicalBallPrefab != null)
        {
            GameObject magicBall = Instantiate(magicalBallPrefab, this.transform.position, Quaternion.identity);

            Vector3 mousePos = GameInput.Instance.GetMousePositionToScreenWorldPoint();
            mousePos.z = 0;

            Vector2 direction = ( mousePos - transform.position).normalized; // ����������� �� ������
            magicBall.GetComponent<MagicalBall>().Initialize(direction); // �������� ����������� � ���������� ���s

            Debug.Log(transform.position);
            Debug.Log(mousePos);
            Debug.Log(direction);
            OnMagicalBallCast?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.LogError("MagicalBall prefab or shootPoint is not assigned.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // ������������, ��� � ����� ���� ���������, ���������� �� ��������� �����
            EnemyEntity enemy = collision.GetComponent<EnemyEntity>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ������� ����
                Debug.Log("Magic ball hit the enemy");
            }
            Destroy(gameObject); // ���������� ��� ����� ������������
        }
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject); // ���������� ��� ����� ������������
        }
       // �� ������ �������� �������������� ��������, ���� ��� ���������� � ������� ���������
       //, ��������, ����������� ���� ��� ������������ � �������������.
    }
}
