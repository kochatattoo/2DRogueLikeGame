using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum AudioName
{
    CLICK,
    OPEN,
    CLOSE
}

public class AudioManager : MonoBehaviour,IManager, IAudioManager
{
    public static bool music = true; //�������� ����������� ������
    public static bool sounds = true; //�������� ����������� ������

    private AudioSource _audioSource;
    private ResourcesLoadManager _resourcesLoadManager;
    private ButtonClickAudio _buttonClickAudio {  get; set; }

    [SerializeField] private AudioPlayer _playerAudio;
    [SerializeField] private AudioClip[] _clips;  // �� ���� ���� ��������� ���������� ��� ( �� �� ����� ���, ���� ������� ��������� ������ ��� ������ ���)
  
    public void StartManager()
    {
        gameObject.SetActive(true);

        _resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>();
        _buttonClickAudio = gameObject.AddComponent<ButtonClickAudio>();
        _buttonClickAudio.StartScript();
        LoadAudioResources();
        InitializeManager();

        _audioSource = GetComponent<AudioSource>();
        _audioSource.mute = false;
        InitializePlayerAudio();
    }

    public void InitializePlayerAudio()
    {
        _playerAudio = FindObjectOfType<AudioPlayer>();
        if (_playerAudio == null)
        {
            return;
        }
        else
        {
            _playerAudio.StartScript();
        }
    }
    public float AudioVolume { get { return _audioSource.volume; } set { value = _audioSource.volume; } }
    public bool StatusMusic()
    {
        return !_audioSource.mute;
    }
    public void SoundOffOn()
    {
        if (_audioSource != null)
        {
            _audioSource.mute = !_audioSource.mute;
        }
    }
    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }
    public float GetVolume()
    {
        return _audioSource.volume;
    }
    public static void SaveSettings()
    {
        PlayerPrefs.SetString("music",music.ToString()); //��������� ��������� ������
        PlayerPrefs.SetString("sounds", sounds.ToString());//��������� ��������� ������
        PlayerPrefs.Save();//��������� ���������
    }
    //����� ������������� ���������
    private void InitializeManager()
    {
        //����� �� ��������� � ������������ ��������� �� PlayerPrefs
        music = System.Convert.ToBoolean(PlayerPrefs.GetString("music", "true"));
        sounds = System.Convert.ToBoolean(PlayerPrefs.GetString("sounds", "true"));
        SceneManager.sceneLoaded += OnSceneLoaded; //�������� �� ������� �������� �����
    }

    private void OnDestroy()
    {
        //������� �� ������� - ��� �� �� ���� ������ ������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //���� ����� ����� ������ ������ ���, ����� ����������� ����� �����
       
        if (_playerAudio != null)
        {
            InitializePlayerAudio(); //���������� ������ ��� ���������
        }
    }
    private void LoadAudioResources()
    {
        _clips = new AudioClip[1];
    }
    public void PlayAudio(AudioName audioName)
    {
        if (_clips[(int)audioName] != null)
        {
            _audioSource.PlayOneShot(_clips[(int)audioName]);
        }
        else
        {
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.PlayNotificationAudio("Error");
            Debug.LogWarning($"��������� ��� {audioName} �� �������� ��� �����������.");
        }
    }
    public void PlayClick()
    {
        _buttonClickAudio.PlayClickAudio();
    }
    public ButtonClickAudio ButtonClickAudio {  get { return _buttonClickAudio; } } // ��������� �����???
}
