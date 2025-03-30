namespace Assets.Scripts.Interfaces
{
    public interface IAudioManager
    {
        bool StatusMusic();
        void SoundOffOn();
        void SetVolume(float volume);
        //Метод для старта менеджера  
        void StartManager();
        float GetVolume();
        void InitializePlayerAudio();
        void PlayAudio(AudioName audioName);

    }
}
