using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using TMPro;

//Класс отвечающий за поведение Врага
public class EnemyAI : MonoBehaviour
{
    //Сериализуем поле начального состояния врага
    [SerializeField] private State _startingState;
    //Поле максимального передвижения 
    [SerializeField] private float _roamingDistanceMax = 7f;
    //Поле минимального передвижения
    [SerializeField] private float _roamingDistanceMin = 3f;
    //Поле времени передвижения
    [SerializeField] private float _roamingTimeMax = 3f;

    //Флаг является ли враг преследующим
    [SerializeField]private bool _isChasingEnemy = false;
    //Переменая на каком расстоянии начинается премледование
    [SerializeField] private float _chasingDistance = 4f;
    //Переменная отвечающая за ускорение
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    //Флаг является ли враг атакующим
    [SerializeField] private bool _isAttackinEnemy = false;
    //Поле расстояния на котором враг начинает атаковать
    [SerializeField] private float _attackingDistance = 4f;
    //Поле частоты атаки
    [SerializeField] private float _attackRate = 2f;
    //Время следующей атаки
    private float _nextAttackTime = 0f;


    //Переменная класса NavMeshAgetn ответственная за построение пути движения
    private NavMeshAgent _navMeshAgent;
    //Переменная состояния (перечисление)
    private State _currentState;
    //Переменная времени движения
    private float _roamingTimer;
    //Переменная отвечающая за позицию куда будет двигаться
    private Vector3 _roamPosition;
    //Переменная отвечающая за позицию откуда двигается
    private Vector3 _startingPosition;


    //Переменная скорости брожения
    private float _roamigSpeed;
    //Переменная скорости преследования
    private float _chasingSpeed;

    //Когда мы хотим проверить направление
    private float _nextCheckDirectionTime = 0f;
    //Частота проверки правильного направления
    private float _checkDirectionDuration = 0.1f;
    //Последнее положение противника
    private Vector3 _lastPosition;


    //Создаем событие Атаки
    public event EventHandler OnEnemyAttack;

    //Условие для определения бежит или нет
    public bool IsRunning
    { get
        {   //Если скорость нашего агента равна 0, то false
            if (_navMeshAgent.velocity == Vector3.zero)
            { return false; }
            //Если нет, то true
            else { return true; }
        }
    }

    //Метод для отслеживания бежит или нет
    /*
    public bool IsRunning()
    {
        //Если скорость нашего агента равна 0, то false
        if(_navMeshAgent.velocity==Vector3.zero)
        {  return false; }
        //Если нет, то true
        else { return true; }
    }   */

    //Состояния
    private enum State
    {
        Idle, //Покой
        Roaming, //Брожение
        Chasing, //Слежение
        Attacking, //Атака
        Death //Смерть
    }

    private void Awake()
    {
        //Кешируем переменную класса NavMeshAgent
        _navMeshAgent=GetComponent<NavMeshAgent>();
        //Запрет вращения
        _navMeshAgent.updateRotation = false;
        //Запрет изменения от места расположения
        _navMeshAgent.updateUpAxis = false;
        //состояние
        _currentState = _startingState;

        //Скорость брожения
        _roamigSpeed = _navMeshAgent.speed;
        //Скорость преследования
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
    }


    private void Update()
    {
        //Проверка статуса
        StateHandler();
        //Проверка повернутости в сторону направления
        MovementDirectionHandler();
    }
    //Метод устанавливающий смерть врага
    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState=State.Death;
    }

   //Метод отслеживания состояния врага
   private void StateHandler()
    {
        //Выбор от состояния
        switch (_currentState)
        {
            //В случае состояния - Движение
            case State.Roaming:
                //переопределяем время с обратным отчетом каждый кадр
                _roamingTimer -= Time.deltaTime;
                //когда время стало меньше нуля
                if (_roamingTimer < 0)
                {
                    //вызываем метод Движение и переопределяем направления
                    Roaming();
                    //снова выставляем время 
                    _roamingTimer = _roamingTimeMax;
                }
                //Метод установки состояния врага
                CheckCurrentState();
                break;
            case State.Chasing:
                //Метод отвечающий за преследование
                ChasingTarget();
                //Метод установки состояния врага
                CheckCurrentState();
                break;
            case State.Attacking:
                //Метод для атаки
                AttackingTarget();
                //Метод установки состояния врага
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default://Состояние по умолчанию - Покой
            case State.Idle:
                break;
        }
    }


    //Метод преследования
    private void ChasingTarget()
    {
        //Устанавливаем нашему агенту точку движения, где находится Player
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }
    //Метод возвращающий скорость 
    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed/_roamigSpeed;
    }

    //Метод установки текущего состояния
    private void CheckCurrentState()
    {
        //Переменная расстояния до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        //Устанавливаем новое состояние - брожения
        State newState = State.Roaming;

        //Если наш враг преследующий
        if (_isChasingEnemy)
        {
            //Если дистанция до игрока меньше чем дистанция начала преследования
            if(distanceToPlayer <= _chasingDistance)
            {
                //Новое состояние - преследование
                newState = State.Chasing;
            }
        }

        //Если наш враг атакующий
        if (_isAttackinEnemy)
        {
            if (distanceToPlayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        //Если ничего нет, то пусть и ничего не меняется
        if(newState!=_currentState)
        {
            //Если новый статус, равен преследованию
            if (newState == State.Chasing)
            {
                //Сбросить наш путь
                _navMeshAgent.ResetPath();
                //Скорость равна скорости преследования
                _navMeshAgent.speed = _chasingSpeed;
            }
            //Если новый статус брожение
            else if (newState == State.Roaming)
            {
                //ВремяБрожения=0
                _roamingTimer = 0f;
                //скорость агента равна скорости брожения
                _navMeshAgent.speed=_roamigSpeed;
            }
            //Если новоый стату атака
            else if (newState == State.Attacking)
            {
                //Сбрасываем путь
                _navMeshAgent.ResetPath();
            }
            //Текущее состояние, равно новому состоянию
            _currentState = newState;
        }
       
    }

    //Метод для атаки
    private void AttackingTarget()
    {
        //Если время больше времени след атаки(ограничиваем скорость атаки нашего врага)
        if (Time.time > _nextAttackTime)
        {
            //Вызываем событие атаки
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            
            //Время след атаки, тек время + частота атаки
            _nextAttackTime = Time.time+_attackRate;
        }

    }

    //Метод для разорота противника в сторону преследования игрока
    private void MovementDirectionHandler()
    {
        //Если время следующей проверки наступило
        if(Time.time >_nextCheckDirectionTime)
        {
            //Если противник бежит
            if(IsRunning)
            {
                //Поворачиваемся в сторону пршлого и текущего положения
                ChangeFacingDirection(_lastPosition,transform.position);
            }
            //Если атакует
            else if(_currentState == State.Attacking)
            {
                //Поворачиваемся в сторону игрока
                ChangeFacingDirection(transform.position,Player.Instance.transform.position);
            }

            //Переприсваиваем последнюю позицию и время след. проверки 
            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time+_checkDirectionDuration;
        }
    }

    //Метод для переопределения движения
    private void Roaming()
    {
        //определяем стартовую позицию, как позицию нахождения на данный момент
        _startingPosition= transform.position;
        //определяем новую позицию для движения
        _roamPosition = GetRoamingPosition();
        //Построение нового пути движения к точке
        _navMeshAgent.SetDestination( _roamPosition );
    }
    //Получение рандомного направления движения
    private Vector3 GetRoamingPosition()
    {
        //возвращаем значение новой позиции для движения
        return _startingPosition+ Utils.GetRandomDir()*UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }
    //Установка спрайта по направлению движения
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        //проверка точки до и точки куда ,по координатам Х
        if(sourcePosition.x>targetPosition.x)
        {
            //Поворот на 180 градусов
            transform.rotation=Quaternion.Euler(0,-180,0);
        }
        else
        {
            //поворот на 0.0.0
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
