using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; } //��������� �������

    public static bool music = true; //�������� ����������� ������
    public static bool sounds = true; //�������� ����������� ������

    private AudioSource _audioSource;
    [SerializeField] AudioPlayer _playerAudio;
    
    private void Awake()
    {
        //������ ��������� ������������� ����������
        if(Instance == null)
        {
            //������ ������ �� ��������� �������
            Instance = this;
            //������ ��� ����� �������, ��� �� ������ �� �����������
            //��� �������� �� ������ �����
            DontDestroyOnLoad(gameObject);
            //� ��������� �������������
            InitializeManager();
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
        FindPlayerAudio();
    }

    public void FindPlayerAudio()
    {
        _playerAudio = FindObjectOfType<AudioPlayer>();
    }
    public bool StatusMusic()
    {
        if (_audioSource.mute == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SoundOffOn()
    {
        if(_audioSource.mute == false)
        {
            _audioSource.mute = true;
        }
        else
        {
            _audioSource.mute = false;
        }
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
        FindPlayerAudio(); //���������� ������ ��� ���������
    }
}
