using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Реализуем класс Меча
public class Sword : MonoBehaviour
{
    //Создаем переменную уронаОтМеча
    [SerializeField] private int _damageAmount = 2;
    //Вызываем событие "Взмах Меча"
    public event EventHandler OnSwordSwing;
    //Вводим переменную Полигон Колайдер 2Д
    private PolygonCollider2D _polygonCollider2D;

    //Метод Авейк, вызывается при первом появлении
    private void Awake()
    {
        //Инициализируем ПолигонКолайдер2Д (Кешируем)
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _polygonCollider2D.enabled = false;
    }

    //Метод Старт, вызывается перед первым методом Апдейт
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
        //Вызываем событие "Взмах меча" и получчаем отбратно значение
        OnSwordSwing?.Invoke(this,EventArgs.Empty);
    }

    //Метод, для проверки не консгулся ли колайдер, другого колайдера в 2д
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Проверяем что ударили по врагу
        //Возвращаем в строку значение типа EnemyEntuty из-за out
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
