using UnityEngine;
using System;
using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Exceptions;

public class MagicalBall : MonoBehaviour
{
    [SerializeField] private GameObject magicalBallPrefab;
    [SerializeField] private MagicalBallVisual magicalBallVisual;

    public event EventHandler OnMagicalBallCast;

    public float speed = 30f; // �������� ������ ����
    public int damage = 2; // ����, ������� ����� ����������

    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D _rigidbody2D;
   
    private Vector3 direction; // ����������� �������� ����

    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

       // _circleCollider2D.enabled = false;
    }

    public void Initialize(Vector3 dir)
    {
        direction = dir;
        magicalBallVisual.SetDirection(direction);

    }
    private void Update()
    {
        // ���� ����������� �� � ������� ���������, ���������� ���
        if (direction != Vector3.zero)
        {
            direction.z = 0;
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
            Vector3 position = this.transform.position;
            position.z = 0;
            GameObject magicBall = Instantiate(magicalBallPrefab, this.transform.position, Quaternion.identity);
            // ������� ������ ������ � ����� �������� � ��������� � ���� ��� � ������������� �������
           // magicBall.transform.SetParent(Player.Instance.transform);
            
            
            var gameInput = ServiceLocator.GetService<IGameInput>();
            Vector3 mousePos = gameInput.GetMousePositionToScreenWorldPoint();
            mousePos.z = 0;

            Vector2 direction = ( mousePos - transform.position).normalized; // ����������� �� ������
            magicBall.GetComponent<MagicalBall>().Initialize(direction); // �������� ����������� � ���������� ���s

           // Debug.Log(transform.position);
           // Debug.Log(mousePos);
           // Debug.Log(direction);
            OnMagicalBallCast?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.PlayNotificationAudio("Error");
            Debug.LogError("MagicalBall prefab or shootPoint is not assigned.");
            throw new CustomException("MagicalBall prefab or shootPoint is not assigned."); // TODO: �� �� ��� �� ����� ��� ��������
        }
    }

    public void DestroyMagicBall()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Collided with: " + collision.name); // ��������, � ��� ���������� ���.

            // ������������, ��� � ����� ���� ���������, ���������� �� ��������� �����
            EnemyEntity enemy = collision.GetComponent<EnemyEntity>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ������� ����
                direction = Vector2.zero;

              //  Debug.Log("Magic ball hit the enemy");
                magicalBallVisual.SetState(MagicalBallVisual.State.Hit);
            }

           // Destroy(gameObject); // ���������� ��� ����� ������������
        }
        else if (!collision.CompareTag("Player"))
        {
            Debug.Log("Collided with: " + collision.name); // ��������, � ��� ���������� ���.
            direction = Vector2.zero;
            magicalBallVisual.SetState(MagicalBallVisual.State.Destroy);

           // Destroy(gameObject); // ���������� ��� ����� ������������
        }
       // �� ������ �������� �������������� ��������, ���� ��� ���������� � ������� ���������
       //, ��������, ����������� ���� ��� ������������ � �������������.
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
