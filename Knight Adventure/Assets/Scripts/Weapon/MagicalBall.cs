using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicalBall : MonoBehaviour
{
    [SerializeField] private GameObject magicalBallPrefab;

    public event EventHandler OnMagicalBallCast;

    public float speed = 30f; // —корость полета шара
    public int damage = 2; // ”рон, который будет наноситьс€

    private CircleCollider2D _circleCollider2D;
   
    private Vector2 direction; // Ќаправление движени€ шара

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
        // ≈сли направление не в нулевом состо€нии, перемещаем шар
        if (direction != Vector2.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime); // ѕеремещение шара
        }
    }
    public void Attack()
    {
        // ћожно сделать что бы начальной позицией была позици€ игрока, но тогда фаерболы лет€т откуда то с ног
        // в атак добавл€ем Vector2 playerPosition и используем данную переменную дл€ хранени€ позиции
        // ј в методе атаки в Player мы  вставим его положение (vector2)transform.position

        if (magicalBallPrefab != null)
        {
            GameObject magicBall = Instantiate(magicalBallPrefab, this.transform.position, Quaternion.identity);

            Vector3 mousePos = GameInput.Instance.GetMousePositionToScreenWorldPoint();
            mousePos.z = 0;

            Vector2 direction = ( mousePos - transform.position).normalized; // Ќаправление на курсор
            magicBall.GetComponent<MagicalBall>().Initialize(direction); // ѕередаем направление в магический шарs

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
            // ѕредполагаем, что у врага есть компонент, отвечающий за получение урона
            EnemyEntity enemy = collision.GetComponent<EnemyEntity>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Ќаносим урон
                Debug.Log("Magic ball hit the enemy");
            }
            Destroy(gameObject); // ”ничтожаем шар после столкновени€
        }
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject); // ”ничтожаем шар после столкновени€
        }
       // ¬ы можете добавить дополнительные действи€, если шар столкнетс€ с другими объектами
       //, например, уничтожение шара при столкновении с преп€тстви€ми.
    }
}
