namespace Assets.Scripts.Interfaces
{
    public interface IAudioManager
    {
        public float AudioVolume {get; set;}
        bool StatusMusic();
        void SoundOffOn();
        void StartManager();
        void InitializePlayerAudio();
        void PlayAudio(AudioName audioName);
        void PlayClick();

    }
}
