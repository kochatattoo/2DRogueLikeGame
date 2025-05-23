using Assets.Scripts.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

//����� ���������� �� ������ ����� 
public class GameInput : MonoBehaviour, IManager, IGameInput
{
    //��������� ���������� ������ InputAction ������� ������������ �������
    //������ ��� ����������
    private PlayerInputActions _playerInputActions;


    //���������� ������� ����� � �����
    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerPause;
    public event EventHandler OnPlayerMagicAttack;
    public event EventHandler OnPlayerRangeAttack;

    public event EventHandler OnPlayerOpen;

    public void StartManager()
    {
        gameObject.SetActive(true);
        //������ playerInputAction, ������ ��� ���������
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        //������������� �� ������� ����� � �����
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Combat.Range_Attack.started += Range_Attack_started;
        _playerInputActions.Combat.Magic_Attack.started += Magic_Attack_started;
        _playerInputActions.Player.Pause.started += PlayerPause_started;
        _playerInputActions.Open.Open.started += Open_started;
    }
    public void DisableManager()
    {
        _playerInputActions.Disable();
        //������������ ������� ����� � �����
        _playerInputActions.Combat.Attack.started -= PlayerAttack_started;
        _playerInputActions.Combat.Range_Attack.started -= Range_Attack_started;
        _playerInputActions.Combat.Magic_Attack.started -= Magic_Attack_started;
        _playerInputActions.Player.Pause.started -= PlayerPause_started;
        _playerInputActions.Open.Open.started -= Open_started;
        _playerInputActions=null;
    }
    private void OnDestroy()
    {
      //  DisableManager();
    }
    private void OnDisable()
    {
      //  DisableManager();
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
    public Vector3 GetMousePositionToScreenWorldPoint()
    {
        // �������� ������� ������� � �������� �����������
        Vector3 mousePos = Input.mousePosition;

        // ����������� �������� ���������� � ������� ����������
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0; // ������������� z � 0 ��� 2D
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
    public void DisablePlayerMovement()
    {
        _playerInputActions.Combat.Attack.started -= PlayerAttack_started;
        _playerInputActions.Combat.Range_Attack.started -= Range_Attack_started;
        _playerInputActions.Combat.Magic_Attack.started -= Magic_Attack_started;
        _playerInputActions.Open.Open.started -= Open_started;
    }
   public void EnablePlayerMovement()
    {
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Combat.Range_Attack.started += Range_Attack_started;
        _playerInputActions.Combat.Magic_Attack.started += Magic_Attack_started;
        _playerInputActions.Open.Open.started += Open_started;
    }
    //������� �����
    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    private void Range_Attack_started(InputAction.CallbackContext obj)
    {
        OnPlayerRangeAttack?.Invoke(this, EventArgs.Empty);
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

    private void Open_started(InputAction.CallbackContext obj)
    {
        OnPlayerOpen?.Invoke(this, EventArgs.Empty);
    }

}
