using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
    //Сериализуем меч
    [SerializeField] private Sword sword;
    //Переменная контроллер анимации
    private Animator animator;
    //Константа, что бы не ошибиться и не вводить каждый раз стринг поле
    private const string ATTACK = "Attack";

    private void Awake()
    {
        //Инициализируем аниматор (кешируем)
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        //Событие взмах меча
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }
    //Вызываем метод взмаха меча через событие
    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        //устанавливаем значение тригера - Attack, в контролере анимации
        animator.SetTrigger(ATTACK);
    }
    //Публичный метод, вызываемый в анимации для выключения полигонколайдера2Д
    //В анимации, добавляем Event в конец анимации и присваиваем ему данный метод
    public void triggerEndAttackAnimation()
    {
        //Метод вызываемый в классе Sword, для отключения колайдера
        sword.AttackCollaiderTurnOff();
    }
}
