```
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.AudioScripts
{
    public class OptionsVolumeController:VolumeControler
    {
        public Slider soundSlider;
        public AudioPlayer audioPlayer;

        private void Start()
        {
            //soundSlider.value = PlayerPrefs.GetFloat("sound_volume", 0.5f);
            audioPlayer=FindObjectOfType<AudioPlayer>();
        }
        public void OnSoundChange()
        {
            float volume = soundSlider.value;
            audioPlayer.SetVolume(volume);
            PlayerPrefs.SetFloat("sound_volume", volume);
        }
    }
}
```