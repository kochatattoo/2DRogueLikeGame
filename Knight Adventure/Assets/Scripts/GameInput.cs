using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Класс отвечающий за логику ввода 
public class GameInput : MonoBehaviour, IGameInput
{
    //Объявляем переменную класса InputAction которая автоматичски создает
    //методы для управления
    private PlayerInputActions _playerInputActions;

    //Создаем наш класс синглтоном, что бы можно было обращатся к его элементам 
    //в других классах
    public static GameInput Instance {  get; private set; }
    //Переменные событий Атака и Пауза
    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerPause;
    public event EventHandler OnPlayerMagicAttack;
    public event EventHandler OnPlayerRangeAttack;

    public event EventHandler OnPlayerOpen;

    private void Awake()
    {
        //Объявляем синглтоном
        Instance = this;

        // Регистрация самого себя в Service Locator
        ServiceLocator.RegisterService<GameInput>(this);

        //вводим playerInputAction, делаем его доступным
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        //Подписываемся на события Атаки и Паузы
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Combat.Range_Attack.started += Range_Attack_started;
        _playerInputActions.Combat.Magic_Attack.started += Magic_Attack_started;
        _playerInputActions.Player.Pause.started += PlayerPause_started;
        _playerInputActions.Open.Open.started += Open_started;
    }

    //Метод отвечающий за передвижение
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    //Метод отвечающий за расположение мыши на экране
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
    public Vector3 GetMousePositionToScreenWorldPoint()
    {
        // Получаем позицию курсора в экранных координатах
        Vector3 mousePos = Input.mousePosition;

        // Преобразуем экранные координаты в мировые координаты
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0; // Устанавливаем z в 0 для 2D
        return mousePos;
    }

    //Метод не позволяющий передвигаться
    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }

    //Метод позволяющий передвигаться
    public void EnableMovement()
    {
        _playerInputActions.Enable();
    }

    //Событие Атаки
    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    private void Range_Attack_started(InputAction.CallbackContext obj)
    {
        OnPlayerRangeAttack?.Invoke(this, EventArgs.Empty);
    }
    //Событие атаки магией
    private void Magic_Attack_started(InputAction.CallbackContext obj)
    {
        OnPlayerMagicAttack?.Invoke(this, EventArgs.Empty);
    }

    //Событие паузы
    private void PlayerPause_started(InputAction.CallbackContext obj)
    {
        OnPlayerPause?.Invoke(this, EventArgs.Empty);
    }

    private void Open_started(InputAction.CallbackContext obj)
    {
        OnPlayerOpen?.Invoke(this, EventArgs.Empty);
    }

}
