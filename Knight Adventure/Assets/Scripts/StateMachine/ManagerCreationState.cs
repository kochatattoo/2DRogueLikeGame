public class ManagerCreationState : IGameState
{
    private InitializationManager _manager;

    public ManagerCreationState(InitializationManager manager)
    {
        _manager = manager;
    }

    public void Enter()
    {
        _manager.CreateAndRegisterManagers();
    }

    public void Exit() { }

    public void Update() { }
}

