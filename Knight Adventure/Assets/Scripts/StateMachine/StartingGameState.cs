public class StartingGameState : IGameState
{
    private InitializationManager _manager;

    public StartingGameState(InitializationManager manager)
    {
        _manager = manager;
    }

    public void Enter()
    {
        _manager.StartGame();
    }

    public void Exit() { }

    public void Update() { }
}
