using Zenject;

namespace Assets.TestZenject.Infrastracture
{
    public class BootstrapInstaller : MonoInstaller
    {
        //В данном классе будут регистрироваться сервисы, которые будут находится в процессе всей игры
        public override void InstallBindings()
        {
           
        }
    }
}
