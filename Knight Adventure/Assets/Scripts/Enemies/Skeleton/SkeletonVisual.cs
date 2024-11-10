using UnityEngine;

//������������� ��������� ����������� ��� ����������
[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(SpriteRenderer))]
//����� ���������� �� ����������� �������� �������
public class SkeletonVisual : MonoBehaviour
{
    //���������� � ������ EnenmyAI ��� ��������� ��� �����������
    [SerializeField] private EnemyAI _enemyAI;
    //���������� � ������ EnemuEntity ��� ��������� ��� �����������
    [SerializeField] private EnemyEntity _enemyEntity;
    //���������� � �������
    [SerializeField] private GameObject _enemyShadow;


    //���������� ��������� 
    private Animator _animator;

    //��������� ���������� � ���������
    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string IS_DIE = "IsDie";
    private const string TAKE_HIT = "TakeHit";

    //��������� ���� ������ ��������
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        //�������� ���������� ��������� � ������ �������
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        //������������� �� ������� ����� � ������ EnemyAI
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        //�������������� �� ������� ��������� ����� � ������ EnemyEntity
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        //������������� �� ������� ������
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }

    //������ ���������� ����� ����������� �������
    private void OnDestroy()
    {
        //������������ �� �������
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath -= _enemyEntity_OnDeath;
    }

    private void Update()
    {
        //������������� �������� isrunning ��� � ������ �������
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        //������������� ��������� �������� ��� ��������� 
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    //����� ��� ��������� ���������������� ��� �����(��������� � ���������)
    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonCollaiderTurnOff();
    }

    //����� ��� ���������� ���������������� ����� �����(��������� � ���������)
    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonCollaiderTurnOn();
    }

    //����� ��� ���������� ������� �����
    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        //�������� ������ ����� � ���������
        _animator.SetTrigger(ATTACK);
    }

    //����� ���������� ��� ������������ �������
    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKE_HIT);
    }

    //����� ���������� ��� ������� ������
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        //���������� ��� �� ��� �� ������ ����� ������
        _spriteRenderer.sortingOrder = -1;
        //������� ����
        _enemyShadow.SetActive(false);
    }
}
