using Zenject;

namespace Assets.TestZenject.Infrastracture
{
    public class BootstrapInstaller : MonoInstaller
    {
        //В данном классе будут регистрироваться сервисы, которые будут находится в процессе всей игры

        public GameInput GameInjputPrefab;
        public override void InstallBindings()
        {
            BindGameInput();
        }

        private void BindGameInput()
        {
            Container
                 .Bind<IGameInput>()
                 .To<GameInput>()
                 .FromComponentInNewPrefab(GameInjputPrefab)
                 .AsSingle();
        }
    }
}
