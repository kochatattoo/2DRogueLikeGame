using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private IGameState _currentState;

    private void Start()
    {
        // Устанавливаем начальное состояние
        ChangeState(new MainMenuState(this));
    }

    private void Update()
    {
        // Обновление текущего состояния
        _currentState?.Update();
    }

    public void ChangeState(IGameState newState)
    {
        // Выход из текущего состояния
        _currentState?.Exit();

        _currentState = newState;

        // Вход в новое состояние
        _currentState.Enter();
    }
}