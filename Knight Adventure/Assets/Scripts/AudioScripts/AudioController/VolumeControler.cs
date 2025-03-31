using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControler : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
    }

    public void OnVolumeChange()
    {
        float volume = volumeSlider.value;
        var audioManager = ServiceLocator.GetService<IAudioManager>();
        audioManager.AudioVolume = volume;

        //AudioManager.Instance.SetVolume(volume);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
