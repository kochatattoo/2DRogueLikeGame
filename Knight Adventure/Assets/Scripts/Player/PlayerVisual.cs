using Assets.ServiceLocator;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    //������� ���������� ����� ������
    [SerializeField] private PlayerAnimationAttack _animatioAttack;
    //��������� ��������� ��������� � �������������
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    /*[SerializeField] Player Player;*/

    //������ ��������� ��������� ��� ���������� ������ � ����������
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";
    private const string TAKE_DAMAGE = "TakeDamage";
    private const string ANIMATION_ATTACK = "AnimationAttack";

    private IGameInput _gameInput;

    private void Awake()
    {
        //�������� �������� � ������������
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        //������������� �� ������� ������ � ��������� ����� ���������
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Player.Instance.OnTakeHit += Player_OnTakeHit;

        _gameInput=ServiceLocator.GetService<IGameInput>();
        _gameInput.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;

    }

    //������� ��������� �����
    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        //�������� ������������� ������� �� ��������� �����
        animator.SetTrigger(TAKE_DAMAGE);

    }

    //������� ������
    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        //���������� ������ ������
        animator.SetBool(IS_DEAD, true);
    }

    private void Update()
    {
        //��������� ����� ��� ����� ��� ��� � ������ �������������� ������
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());

        //���� ��� �������� �����
        if (Player.Instance.IsAlive())
        {
            //������ �� ������������ ����
            AdjustPlayerFacingDirection();
        }
    }

    //������� ���������� �����
    private void Player_OnPlayerMagicAttack(object sender, System.EventArgs e)
    {
        if (Player.Instance.IsAlive())
        //������������� ������ ���������� 
        { 
            animator.SetTrigger(ANIMATION_ATTACK);
        }
    }

    //������ �� ����������� ���� �� ������ � ������������ ��������� � �� �������
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos=_gameInput.GetMousePosition();
        Vector3 playerPos=Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPos.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    public void triggerEndAttackAnimation()
    {
        //����� ���������� � ������ Sword, ��� ���������� ���������
        _animatioAttack.AttackCollaiderTurnOff();
    }

}
