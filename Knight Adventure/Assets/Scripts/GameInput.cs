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
    public event EventHandler _OnPlayerAttack;
    public event EventHandler _OnPlayerPause;

    private void Awake()
    {
        //��������� ����������
        Instance = this;

        //������ playerInputAction, ������ ��� ���������
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        //������������� �� ������� ����� � �����
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
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
        _OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    //������� �����
    private void PlayerPause_started(InputAction.CallbackContext obj)
    {
        _OnPlayerPause?.Invoke(this, EventArgs.Empty);
    }

}
