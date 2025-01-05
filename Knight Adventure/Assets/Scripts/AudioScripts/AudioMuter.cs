using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;

public class AudioMuter : MonoBehaviour
{
    [SerializeField] private GameObject _soundOn;
    [SerializeField] private GameObject _soundOff;

    private AudioSource _musicAudioSource;
    private bool music;
    
    private void Start()
    {
        _musicAudioSource = GetComponent<AudioSource>();
    }

    public void MuteSound()
    {
        var audioManager = ServiceLocator.GetService<IAudioManager>();
        audioManager.SoundOffOn();
        //AudioManager.Instance.SoundOffOn();
        SwitchSoundImage();
    }

    private void SwitchSoundImage()
    {
        var audioManager = ServiceLocator.GetService<IAudioManager>();
        music=audioManager.StatusMusic();

       // music = AudioManager.Instance.StatusMusic();

        if (music)
        {
            _soundOn.SetActive(false);
            _soundOff.SetActive(true);
        }
        else
        {
            _soundOn.SetActive(true);
            _soundOff.SetActive(false);
        }
    }
}
