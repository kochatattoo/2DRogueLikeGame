using Assets.ServiceLocator;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    //Объявляем переменные классов Меч и Магия
    [SerializeField] private Sword sword;
    [SerializeField] private PlayerAnimationAttack _magic;
    [SerializeField] private MagicalBall _magicalBall;

    private IGameInput _gameInput;

    private void Start()
    {
       _gameInput = ServiceLocator.GetService<IGameInput>();
    }

    private void Update()
    {
        //Если игрок жив, то следуюем за игроком
        if (Player.Instance.IsAlive)
        { 
            FollowMousePosition();
        }
    }

    //Методы для получения активного орудия атаки
    public Sword GetActiveWeapon()
    { return sword; }
    public PlayerAnimationAttack GetMagicWeapon() 
    { return _magic; }
    public MagicalBall GetMagicalBall()
    { return _magicalBall; }

    private void FollowMousePosition()
    {
        Vector3 mousePos = _gameInput.GetMousePosition();
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPos.x)
        {
            transform.rotation=Quaternion.Euler(0,180,0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}

