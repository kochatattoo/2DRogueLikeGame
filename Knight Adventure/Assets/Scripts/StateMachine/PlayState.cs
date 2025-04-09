using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
public class PlayState : IGameState
{
    private InitializationManager _initializationManager;
    
    public PlayState(InitializationManager initManager)
    {
        _initializationManager = initManager;
    }

    public void Enter()
    {
        _initializationManager.EnableGameManagers();
    }

    public void Update()
    {
          
    }

    public void Exit()
    {
        _initializationManager.DisableGameManagers();
        Debug.Log("Exiting Play State");
    }
}

