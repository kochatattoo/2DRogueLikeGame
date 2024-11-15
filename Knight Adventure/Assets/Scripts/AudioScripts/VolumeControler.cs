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
        AudioManager.Instance.SetVolume(volume);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
