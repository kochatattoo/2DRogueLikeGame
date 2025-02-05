using Assets.ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    //������ �� ������ ��������
    public static ActiveWeapon Instance { get; private set; }

    //��������� ���������� ������� ��� � �����
    [SerializeField] private Sword sword;
    [SerializeField] private PlayerAnimationAttack _magic;
    [SerializeField] private MagicalBall _magicalBall;

    private IGameInput _gameInput;
    private void Awake()
    {
        //��������
        Instance = this;
    }
    private void Start()
    {
       _gameInput = ServiceLocator.GetService<IGameInput>();
    }

    private void Update()
    {
        //���� ����� ���, �� �������� �� �������
        if (Player.Instance.IsAlive())
        { 
            FollowMousePosition();
        }
    }

    //������ ��� ��������� ��������� ������ �����
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

