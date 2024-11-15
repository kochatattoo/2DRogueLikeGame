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
        AudioManager.Instance.SoundOffOn();
        SwitchSoundImage();
    }

    private void SwitchSoundImage()
    {
        music = AudioManager.Instance.StatusMusic();

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
