using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Класс отвечающий за логику ввода 
public class GameInput : MonoBehaviour
{
    //Объявляем переменную класса InputAction которая автоматичски создает
    //методы для управления
    private PlayerInputActions _playerInputActions;

    //Создаем наш класс синглтоном, что бы можно было обращатся к его элементам 
    //в других классах
    public static GameInput Instance {  get; private set; }
    //Переменные событий Атака и Пауза
    public event EventHandler _OnPlayerAttack;
    public event EventHandler _OnPlayerPause;

    private void Awake()
    {
        //Объявляем синглтоном
        Instance = this;

        //вводим playerInputAction, делаем его доступным
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        //Подписываемся на события Атаки и Паузы
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Player.Pause.started += PlayerPause_started;
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
        _OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    //Событие паузы
    private void PlayerPause_started(InputAction.CallbackContext obj)
    {
        _OnPlayerPause?.Invoke(this, EventArgs.Empty);
    }

}
