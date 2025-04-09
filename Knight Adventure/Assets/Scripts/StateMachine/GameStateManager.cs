using UnityEngine;

public class GameStateManager 
{
    private IGameState _currentState;
    private MainMenuManager _mainMenuManager;

    private void Start()
    {
       
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