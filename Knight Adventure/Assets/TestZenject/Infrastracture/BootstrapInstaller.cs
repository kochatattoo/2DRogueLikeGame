using Zenject;
using Assets.Scripts.Interfaces;

namespace Assets.TestZenject.Infrastracture
{
    public class BootstrapInstaller : MonoInstaller
    {
        //В данном классе будут регистрироваться сервисы, которые будут находится в процессе всей игры
        public AudioManager AudioManager;
        public SaveManager SaveManager;
        public GameManager GameManager;
        

        public override void InstallBindings()
        {
           
        }

       
      
    }
}
