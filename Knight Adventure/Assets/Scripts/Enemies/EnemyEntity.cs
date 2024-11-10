using System;
using UnityEngine;

//Автоматически добавит компонент полигонколайдер, даже если его не будет в инспекторе
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent (typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    //Объявляем события Получение урона и Смерть
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    //Объявляем переменную отвечающую за текущее здоровье
    private int _currentHealth;

    //переменная полигон колайдера
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        //Кешируем полигонколайдер
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _polygonCollider2D.enabled = false;
        //Кешируем боксколайдер
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.enabled = true;

        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        //При старте присваиваем значение макс к текущему
        _currentHealth=_enemySO.enemyHealth;
    }

    //Метод при пересечении коллайдера
    private void OnTriggerStay2D(Collider2D collision)
    {
       if (collision.transform.TryGetComponent(out Player player))
        {
            //Вызываем метод поолучения урона главным героем
            player.TakeDamage(transform, _enemySO.EnemyDamageAmount);
            //PlayerHealthManager.Instance.TakeDamage(_enemySO.EnemyDamageAmount);
        }
    }

    //Метод отвечающий за получение урона
    public void TakeDamage(int damage)
    {
        //Переприсваиваем значение текущего здоровья
        _currentHealth-=damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        //Отслеживаем состояние смерти
        DetectDeth();
    }

    //Метод выключающий полигонколайдер
    public void PolygonCollaiderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    //Метод включающий полигонколайдер
    public void PolygonCollaiderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    //Метод отслеживающий смерть врага
    private void DetectDeth()
    {
        //Если здоровье равно 0, то уничтожаем объект
        if (_currentHealth <= 0) {
            //Вызываем событие смерть
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;

            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

}
