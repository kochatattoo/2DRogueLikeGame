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
            // Получаем уровень громкости из PlayerPrefs и устанавливаем его на слайдер
            float savedVolume = PlayerPrefs.GetFloat("sound_volume", 0.5f); // Значение по умолчанию 0.5f
                                                                            // 
            audioPlayer = FindObjectOfType<AudioPlayer>(); // Находим AudioPlayer
            soundSlider.value = savedVolume; // Устанавливаем значение слайдера
            audioPlayer.SetVolume(savedVolume); // Устанавливаем начальный уровень громкости
        }
        public void OnSoundChange()
        {
            float volume = soundSlider.value;
            audioPlayer.SetVolume(volume);
            PlayerPrefs.SetFloat("sound_volume", volume);
        }
    }
}
