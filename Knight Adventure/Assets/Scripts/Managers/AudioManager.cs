using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; } //��������� �������

    public static bool music = true; //�������� ����������� ������
    public static bool sounds = true; //�������� ����������� ������

    private AudioSource _audioSource;
    
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
            return;
        }
        else 
        {
            //������� ������
            Destroy(this.gameObject);
        }

        //� ��������� �������������
        InitializeManager();
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.mute = false;
       
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
    }
}
