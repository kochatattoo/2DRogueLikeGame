using Zenject;
using UnityEngine;

namespace Assets.TestZenject.Infrastracture
{
    public class LocationInstaller : MonoInstaller
    {
        //В данном классе будут регистрироваться сервисы, которые будут жить только на определенной сцене

        public Transform StartPoint;
        public GameObject HeroPrefab;

        public override void InstallBindings()
        {
            Player Hero = Container
                .InstantiatePrefabForComponent<Player>(HeroPrefab, StartPoint.position, Quaternion.identity, null);

            // Регистрировать серивисы удобно по Интерфейсам, а не самой реализации
            Container
                .Bind<Player>()
                .FromInstance(Hero)
                .AsSingle();
        }
    }
}

