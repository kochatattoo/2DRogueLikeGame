using UnityEngine;

public class PlayerAnimationAttack : MonoBehaviour
{
    //Создаем переменную количество урона 
    [SerializeField] private int _damageAmount = 5;
    //Вводим переменную полигон колайдера 2д
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        //Инициализируем полигонколайдер (кешируем)
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _polygonCollider2D.enabled = false;
    }

    private void Start()
    {
        //Выключаем полигонколайдер в самом начале
        AttackCollaiderTurnOff();
    }

    //Метод, вызывающий атаку
    public void Attack()
    {
        //Включаем полигонколайдер2д при атаке
        AttackCollaiderOffOn();
     
    }

    //Метод, для проверки не коcнулся ли колайдер, другого колайдера в 2д
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Проверяем что ударили по врагу
        //Возвращаем в строку значение типа EnemyEntity из-за out
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            //Вызываем метод ПолучениеУрона у enemyEntity 
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    //Метод, выключающий полигонколайдер2д
    public void AttackCollaiderTurnOff()
    {
        //Выключаем Полигонколайдер2Д
        _polygonCollider2D.enabled = false;
    }
    //Метод, включающий полигонколайдер2д
    private void AttackCollaiderTurnOn()
    {
        //Включаем полигонколайдер2Д
        _polygonCollider2D.enabled = true;
    }

    //Метод для того, что бы при быстром нажатии и взмахе
    //Успевал реализовываться удар по противнику, даже если 
    //анимация не успела закончится и началась следующая
    private void AttackCollaiderOffOn()
    {
        AttackCollaiderTurnOff();
        AttackCollaiderTurnOn();
    }
}
