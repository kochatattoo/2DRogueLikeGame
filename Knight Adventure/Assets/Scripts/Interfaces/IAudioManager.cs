using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IAudioManager
    {
        bool StatusMusic();
        void SoundOffOn();
        void SetVolume(float volume);
        //Метод для старта менеджера  
        void StartManager();
    }
}
