using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private IGameState _currentState;
    private MainMenuManager _mainMenuManager;

    private void Start()
    {
        _mainMenuManager = FindObjectOfType<MainMenuManager>();
        // Устанавливаем начальное состояние
        ChangeState(new MainMenuState(this, _mainMenuManager));
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
    public MainMenuManager FindMainMenu()
    {
        var findObect = FindObjectOfType<MainMenuManager>();
        return findObect;
    }

    public T FindObject<T>() where T : MonoBehaviour
    {
        T foundObject = GameObject.FindObjectOfType<T>();

        if (foundObject == null)
        {
            Debug.LogWarning($"Object of type {typeof(T)} not found!");
        }

        return foundObject;
    }
}