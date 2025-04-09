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

    public float speed = 30f; // Скорость полета шара
    public int damage = 2; // Урон, который будет наноситься

    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D _rigidbody2D;
   
    private Vector3 direction; // Направление движения шара

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
        // Если направление не в нулевом состоянии, перемещаем шар
        if (direction != Vector3.zero)
        {
            direction.z = 0;
            transform.Translate(direction * speed * Time.deltaTime); // Перемещение шара
        }
    }
    public void Attack()
    {
        // Можно сделать что бы начальной позицией была позиция игрока, но тогда фаерболы летят откуда то с ног
        // в атак добавляем Vector2 playerPosition и используем данную переменную для хранения позиции
        // А в методе атаки в Player мы  вставим его положение (vector2)transform.position

        if (magicalBallPrefab != null)
        {
            Vector3 position = this.transform.position;
            position.z = 0;
            GameObject magicBall = Instantiate(magicalBallPrefab, this.transform.position, Quaternion.identity);
            // Сделать пустой объект в точке выстрела и привязать к нему как к родительскому объекту
           // magicBall.transform.SetParent(Player.Instance.transform);
            
            
            var gameInput = ServiceLocator.GetService<IGameInput>();
            Vector3 mousePos = gameInput.GetMousePositionToScreenWorldPoint();
            mousePos.z = 0;

            Vector2 direction = ( mousePos - transform.position).normalized; // Направление на курсор
            magicBall.GetComponent<MagicalBall>().Initialize(direction); // Передаем направление в магический шарs

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
            throw new CustomException("MagicalBall prefab or shootPoint is not assigned."); // TODO: не то что бы понял как работает
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
            Debug.Log("Collided with: " + collision.name); // Сообщает, с чем столкнулся шар.

            // Предполагаем, что у врага есть компонент, отвечающий за получение урона
            EnemyEntity enemy = collision.GetComponent<EnemyEntity>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Наносим урон
                direction = Vector2.zero;

              //  Debug.Log("Magic ball hit the enemy");
                magicalBallVisual.SetState(MagicalBallVisual.State.Hit);
            }

           // Destroy(gameObject); // Уничтожаем шар после столкновения
        }
        else if (!collision.CompareTag("Player"))
        {
            Debug.Log("Collided with: " + collision.name); // Сообщает, с чем столкнулся шар.
            direction = Vector2.zero;
            magicalBallVisual.SetState(MagicalBallVisual.State.Destroy);

           // Destroy(gameObject); // Уничтожаем шар после столкновения
        }
       // Вы можете добавить дополнительные действия, если шар столкнется с другими объектами
       //, например, уничтожение шара при столкновении с препятствиями.
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
