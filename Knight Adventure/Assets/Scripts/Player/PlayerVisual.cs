using Assets.ServiceLocator;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    //Объявим переменную атаку магией
    [SerializeField] private PlayerAnimationAttack _animatioAttack;
    //Объявляем перменные аниматора и спрайтРендера
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    /*[SerializeField] Player Player;*/

    //Опишем текстовые константы для упрощенной работы с аниматором
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";
    private const string TAKE_DAMAGE = "TakeDamage";
    private const string ANIMATION_ATTACK = "AnimationAttack";

    private IGameInput _gameInput;

    private void Awake()
    {
        //Кешируем аниматор и спрайтрендер
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        //Подписываемся на события Смерти и Получения урона персонажа
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Player.Instance.OnTakeHit += Player_OnTakeHit;

        _gameInput=ServiceLocator.GetService<IGameInput>();
        _gameInput.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;

    }

    //Событие получение урона
    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        //Аниматор устанавливает Триггер на получание урона
        animator.SetTrigger(TAKE_DAMAGE);

    }

    //Событие смерти
    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        //Выставляем статус смерти
        animator.SetBool(IS_DEAD, true);
    }

    private void Update()
    {
        //Проверяем бежит наш герой или нет и ставим соотвествующий статус
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());

        //Если наш персонаж живой
        if (Player.Instance.IsAlive())
        {
            //Следим за направлением мыши
            AdjustPlayerFacingDirection();
        }
    }

    //Событие магической атаки
    private void Player_OnPlayerMagicAttack(object sender, System.EventArgs e)
    {
        if (Player.Instance.IsAlive())
        //Устанавливаем тригер анимамтора 
        { 
            animator.SetTrigger(ANIMATION_ATTACK);
        }
    }

    //Следим за нахождением мыши на экране и попорачиваем персонажа в ее сторону
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
        //Метод вызываемый в классе Sword, для отключения колайдера
        _animatioAttack.AttackCollaiderTurnOff();
    }

}
