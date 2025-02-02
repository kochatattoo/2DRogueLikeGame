using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour, IAudioManager
{
    public static AudioManager Instance {  get; private set; } //��������� �������

    public static bool music = true; //�������� ����������� ������
    public static bool sounds = true; //�������� ����������� ������

    private AudioSource _audioSource;
    [SerializeField] AudioPlayer _playerAudio;
    
    private void Awake()
    {
        //������ ��������� ������������� ����������
        if (Instance == null)
        {
            //������ ������ �� ��������� �������
            Instance = this;
            //������ ��� ����� �������, ��� �� ������ �� �����������
            //��� �������� �� ������ �����
            DontDestroyOnLoad(gameObject);
            //� ��������� �������������
           // InitializeManager();
            return;
        }
        else
        {
            //������� ������
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.mute = false;
        InitializePlayerAudio();
    }
    public void StartManager()
    {
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
    public bool StatusMusic()
    {
        return !_audioSource.mute;
    }

    public void SoundOffOn()
    {
        _audioSource.mute = !_audioSource.mute;
    }
    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
        
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
        Debug.Log("Scene Loaded^ " + scene.name);
        if (_playerAudio != null)
        {
            InitializePlayerAudio(); //���������� ������ ��� ���������
        }
    }
}
