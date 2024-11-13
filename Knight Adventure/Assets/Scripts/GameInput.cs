using System;
using UnityEngine;
using UnityEngine.InputSystem;

//����� ���������� �� ������ ����� 
public class GameInput : MonoBehaviour
{
    //��������� ���������� ������ InputAction ������� ������������ �������
    //������ ��� ����������
    private PlayerInputActions _playerInputActions;

    //������� ��� ����� ����������, ��� �� ����� ���� ��������� � ��� ��������� 
    //� ������ �������
    public static GameInput Instance {  get; private set; }
    //���������� ������� ����� � �����
    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerPause;
    public event EventHandler OnPlayerMagicAttack;

    private void Awake()
    {
        //��������� ����������
        Instance = this;

        //������ playerInputAction, ������ ��� ���������
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        //������������� �� ������� ����� � �����
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Combat.Player_animation_attack.started += Magic_Attack_started;
        _playerInputActions.Player.Pause.started += PlayerPause_started;
    }

  

    //����� ���������� �� ������������
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    //����� ���������� �� ������������ ���� �� ������
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    //����� �� ����������� �������������
    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }

    //����� ����������� �������������
    public void EnableMovement()
    {
        _playerInputActions.Enable();
    }

    //������� �����
    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    //������� ����� ������
    private void Magic_Attack_started(InputAction.CallbackContext obj)
    {
        OnPlayerMagicAttack?.Invoke(this, EventArgs.Empty);
    }

    //������� �����
    private void PlayerPause_started(InputAction.CallbackContext obj)
    {
        OnPlayerPause?.Invoke(this, EventArgs.Empty);
    }

}
