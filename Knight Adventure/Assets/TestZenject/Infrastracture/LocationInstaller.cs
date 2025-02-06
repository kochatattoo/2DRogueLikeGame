using Zenject;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.TestZenject.Infrastracture
{
    public class LocationInstaller : MonoInstaller
    {
        //В данном классе будут регистрироваться сервисы, которые будут жить только на определенной сцене

        public Transform StartPoint;
        public GameObject HeroPrefab;
        public GameInput GameInjputPrefab;
        public GUIManager GUIManager;
        public MapManager MapManager;
        public StartScreenManager StartScreenManager;

        public override void InstallBindings()
        {
            BindGameInput();
            BindGUIManager();
            BindingPlayer();
        }

        private void BindingPlayer()
        {
            Player Hero = Container
                           .InstantiatePrefabForComponent<Player>(HeroPrefab, StartPoint.position, Quaternion.identity, null);

            // Регистрировать серивисы удобно по Интерфейсам, а не самой реализации
            Container
                .Bind<Player>()
                .FromInstance(Hero)
                .AsSingle();
        }
        private void BindGameInput()
        {
            Container
                 .Bind<IGameInput>()
                 .To<GameInput>()
                 .FromComponentInNewPrefab(GameInjputPrefab)
                 .AsSingle();
        }
        private void BindGUIManager()
        {
            Container
                .Bind<IGUIManager>()
                .To<GUIManager>()
                .FromComponentInNewPrefab(GUIManager)
                .AsSingle();
        }
        private void BindMapManager()
        {
            Container
                .Bind<IMapManager>()
                .To<MapManager>()
                .FromComponentInNewPrefab(MapManager)
                .AsSingle();
        }
        private void BindStartScreenManager()
        {
            Container
                .Bind<IStartScreenManager>()
                .To<StartScreenManager>()
                .FromComponentInNewPrefab(StartScreenManager)
                .AsSingle();
        }
    }
}

