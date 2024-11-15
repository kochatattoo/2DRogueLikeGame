using UnityEngine;

//[RequireComponent(typeof(Player))]
//[RequireComponent (typeof(AudioSource))]

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _playerAudioSource;
    [SerializeField] private Player _player;

    private void Start()
    {
        _playerAudioSource = GetComponent<AudioSource>();
        _playerAudioSource.mute = true;
    }

    private void Update()
    {
        if(_player.IsRunning())
        {
            _playerAudioSource.mute = false;
        }
        else
        {
            _playerAudioSource.mute = true;
        }
    }
}
