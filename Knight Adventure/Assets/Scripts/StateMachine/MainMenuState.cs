using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState: IGameState
    {

    private InitializationManager _initializationManager;

    public MainMenuState(InitializationManager initializationManager)
    {
        _initializationManager = initializationManager;
    }
    public void Enter()
    {
        _initializationManager.EnableMenuManagers();
    }
    public void Update()
    {
          
    }

    public void Exit()
    {
        _initializationManager?.DisableMenuManagers();
    }
}

