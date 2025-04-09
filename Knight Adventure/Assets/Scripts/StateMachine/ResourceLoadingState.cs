public class ResourceLoadingState : IGameState
{
    private InitializationManager _manager;

    public ResourceLoadingState(InitializationManager manager)
    {
        _manager = manager;
    }

    public void Enter()
    {
       // _manager.LoadResources();
    }

    public void Exit()
    {
        // Здесь можно добавить логику выхода из состояния, если нужно
    }

    public void Update()
    {
        // Логика обновления, если она нужна
    }
}
