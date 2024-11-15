using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _playerAudioSource;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private Player _player;

    [SerializeField] float _stepInterval = 0.5f;
    private float _stepTimer;

    private void Start()
    {
        if (_playerAudioSource == null)
        {
            _playerAudioSource = GetComponent<AudioSource>();
        }

        if (_player == null)
        {
            Debug.LogError("Player is not assigned!");
            return;
        }

        _stepTimer = _stepInterval;

        _player.OnPlayerDeath += Player_OnPlayerDeath;
        _player.OnTakeHit += Player_OnTakeHit;

        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        GameInput.Instance.OnPlayerMagicAttack += Player_OnPlayerMagicAttack;
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (_audioClips.Length > 1 && _audioClips[3] != null)
        {
            _playerAudioSource.PlayOneShot(_audioClips[3]);
        }
    }

    private void Player_OnPlayerMagicAttack(object sender, System.EventArgs e)
    {
        if (_audioClips.Length > 1 && _audioClips[4] != null)
        {
            _playerAudioSource.PlayOneShot(_audioClips[4]);
        }
    }

    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        if (_audioClips.Length > 1 && _audioClips[1] != null)
        {
            _playerAudioSource.PlayOneShot(_audioClips[1]);
        }
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        if (_audioClips.Length > 2 && _audioClips[2] != null)
        {
            _playerAudioSource.PlayOneShot(_audioClips[2]);
        }
    }

    private void Update()
    {
        if (_player != null && _player.IsRunning())
        {
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0)
            {
                PlayFootSepSound();
                _stepTimer = _stepInterval;
            }
        }
    }

    private void PlayFootSepSound()
    {
        if (_audioClips.Length > 0 && _audioClips[0] != null)
        {
            _playerAudioSource.PlayOneShot(_audioClips[0]);
        }
    }
}
