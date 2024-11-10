using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using TMPro;

//����� ���������� �� ��������� �����
public class EnemyAI : MonoBehaviour
{
    //����������� ���� ���������� ��������� �����
    [SerializeField] private State _startingState;
    //���� ������������� ������������ 
    [SerializeField] private float _roamingDistanceMax = 7f;
    //���� ������������ ������������
    [SerializeField] private float _roamingDistanceMin = 3f;
    //���� ������� ������������
    [SerializeField] private float _roamingTimeMax = 3f;

    //���� �������� �� ���� ������������
    [SerializeField]private bool _isChasingEnemy = false;
    //��������� �� ����� ���������� ���������� �������������
    [SerializeField] private float _chasingDistance = 4f;
    //���������� ���������� �� ���������
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    //���� �������� �� ���� ���������
    [SerializeField] private bool _isAttackinEnemy = false;
    //���� ���������� �� ������� ���� �������� ���������
    [SerializeField] private float _attackingDistance = 4f;
    //���� ������� �����
    [SerializeField] private float _attackRate = 2f;
    //����� ��������� �����
    private float _nextAttackTime = 0f;


    //���������� ������ NavMeshAgetn ������������� �� ���������� ���� ��������
    private NavMeshAgent _navMeshAgent;
    //���������� ��������� (������������)
    private State _currentState;
    //���������� ������� ��������
    private float _roamingTimer;
    //���������� ���������� �� ������� ���� ����� ���������
    private Vector3 _roamPosition;
    //���������� ���������� �� ������� ������ ���������
    private Vector3 _startingPosition;


    //���������� �������� ��������
    private float _roamigSpeed;
    //���������� �������� �������������
    private float _chasingSpeed;

    //����� �� ����� ��������� �����������
    private float _nextCheckDirectionTime = 0f;
    //������� �������� ����������� �����������
    private float _checkDirectionDuration = 0.1f;
    //��������� ��������� ����������
    private Vector3 _lastPosition;


    //������� ������� �����
    public event EventHandler OnEnemyAttack;

    //������� ��� ����������� ����� ��� ���
    public bool IsRunning
    { get
        {   //���� �������� ������ ������ ����� 0, �� false
            if (_navMeshAgent.velocity == Vector3.zero)
            { return false; }
            //���� ���, �� true
            else { return true; }
        }
    }

    //����� ��� ������������ ����� ��� ���
    /*
    public bool IsRunning()
    {
        //���� �������� ������ ������ ����� 0, �� false
        if(_navMeshAgent.velocity==Vector3.zero)
        {  return false; }
        //���� ���, �� true
        else { return true; }
    }   */

    //���������
    private enum State
    {
        Idle, //�����
        Roaming, //��������
        Chasing, //��������
        Attacking, //�����
        Death //������
    }

    private void Awake()
    {
        //�������� ���������� ������ NavMeshAgent
        _navMeshAgent=GetComponent<NavMeshAgent>();
        //������ ��������
        _navMeshAgent.updateRotation = false;
        //������ ��������� �� ����� ������������
        _navMeshAgent.updateUpAxis = false;
        //���������
        _currentState = _startingState;

        //�������� ��������
        _roamigSpeed = _navMeshAgent.speed;
        //�������� �������������
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
    }


    private void Update()
    {
        //�������� �������
        StateHandler();
        //�������� ������������ � ������� �����������
        MovementDirectionHandler();
    }
    //����� ��������������� ������ �����
    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState=State.Death;
    }

   //����� ������������ ��������� �����
   private void StateHandler()
    {
        //����� �� ���������
        switch (_currentState)
        {
            //� ������ ��������� - ��������
            case State.Roaming:
                //�������������� ����� � �������� ������� ������ ����
                _roamingTimer -= Time.deltaTime;
                //����� ����� ����� ������ ����
                if (_roamingTimer < 0)
                {
                    //�������� ����� �������� � �������������� �����������
                    Roaming();
                    //����� ���������� ����� 
                    _roamingTimer = _roamingTimeMax;
                }
                //����� ��������� ��������� �����
                CheckCurrentState();
                break;
            case State.Chasing:
                //����� ���������� �� �������������
                ChasingTarget();
                //����� ��������� ��������� �����
                CheckCurrentState();
                break;
            case State.Attacking:
                //����� ��� �����
                AttackingTarget();
                //����� ��������� ��������� �����
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default://��������� �� ��������� - �����
            case State.Idle:
                break;
        }
    }


    //����� �������������
    private void ChasingTarget()
    {
        //������������� ������ ������ ����� ��������, ��� ��������� Player
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }
    //����� ������������ �������� 
    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed/_roamigSpeed;
    }

    //����� ��������� �������� ���������
    private void CheckCurrentState()
    {
        //���������� ���������� �� ������
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        //������������� ����� ��������� - ��������
        State newState = State.Roaming;

        //���� ��� ���� ������������
        if (_isChasingEnemy)
        {
            //���� ��������� �� ������ ������ ��� ��������� ������ �������������
            if(distanceToPlayer <= _chasingDistance)
            {
                //����� ��������� - �������������
                newState = State.Chasing;
            }
        }

        //���� ��� ���� ���������
        if (_isAttackinEnemy)
        {
            if (distanceToPlayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        //���� ������ ���, �� ����� � ������ �� ��������
        if(newState!=_currentState)
        {
            //���� ����� ������, ����� �������������
            if (newState == State.Chasing)
            {
                //�������� ��� ����
                _navMeshAgent.ResetPath();
                //�������� ����� �������� �������������
                _navMeshAgent.speed = _chasingSpeed;
            }
            //���� ����� ������ ��������
            else if (newState == State.Roaming)
            {
                //�������������=0
                _roamingTimer = 0f;
                //�������� ������ ����� �������� ��������
                _navMeshAgent.speed=_roamigSpeed;
            }
            //���� ������ ����� �����
            else if (newState == State.Attacking)
            {
                //���������� ����
                _navMeshAgent.ResetPath();
            }
            //������� ���������, ����� ������ ���������
            _currentState = newState;
        }
       
    }

    //����� ��� �����
    private void AttackingTarget()
    {
        //���� ����� ������ ������� ���� �����(������������ �������� ����� ������ �����)
        if (Time.time > _nextAttackTime)
        {
            //�������� ������� �����
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            
            //����� ���� �����, ��� ����� + ������� �����
            _nextAttackTime = Time.time+_attackRate;
        }

    }

    //����� ��� �������� ���������� � ������� ������������� ������
    private void MovementDirectionHandler()
    {
        //���� ����� ��������� �������� ���������
        if(Time.time >_nextCheckDirectionTime)
        {
            //���� ��������� �����
            if(IsRunning)
            {
                //�������������� � ������� ������� � �������� ���������
                ChangeFacingDirection(_lastPosition,transform.position);
            }
            //���� �������
            else if(_currentState == State.Attacking)
            {
                //�������������� � ������� ������
                ChangeFacingDirection(transform.position,Player.Instance.transform.position);
            }

            //��������������� ��������� ������� � ����� ����. �������� 
            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time+_checkDirectionDuration;
        }
    }

    //����� ��� ��������������� ��������
    private void Roaming()
    {
        //���������� ��������� �������, ��� ������� ���������� �� ������ ������
        _startingPosition= transform.position;
        //���������� ����� ������� ��� ��������
        _roamPosition = GetRoamingPosition();
        //���������� ������ ���� �������� � �����
        _navMeshAgent.SetDestination( _roamPosition );
    }
    //��������� ���������� ����������� ��������
    private Vector3 GetRoamingPosition()
    {
        //���������� �������� ����� ������� ��� ��������
        return _startingPosition+ Utils.GetRandomDir()*UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }
    //��������� ������� �� ����������� ��������
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        //�������� ����� �� � ����� ���� ,�� ����������� �
        if(sourcePosition.x>targetPosition.x)
        {
            //������� �� 180 ��������
            transform.rotation=Quaternion.Euler(0,-180,0);
        }
        else
        {
            //������� �� 0.0.0
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
