using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour, IAudioManager
{
    public static AudioManager Instance {  get; private set; } //Экземпляр объекта

    public static bool music = true; //Параметр доступности музыки
    public static bool sounds = true; //Параметр доступности звуков

    private AudioSource _audioSource;
    [SerializeField] AudioPlayer _playerAudio;
    
    private void Awake()
    {
        //Теперь проверяем существование экземпляра
        if (Instance == null)
        {
            //Задаем ссылку на экземпляр объекта
            Instance = this;
            //Теперь нам нужно указать, что бы объект не уничтожался
            //При переходе на другую сцену
            DontDestroyOnLoad(gameObject);
            //И запускаем инициализатор
           // InitializeManager();
            return;
        }
        else
        {
            //Удаляем объект
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
        PlayerPrefs.SetString("music",music.ToString()); //применяем параметры музыки
        PlayerPrefs.SetString("sounds", sounds.ToString());//Применяем параметры звуков
        PlayerPrefs.Save();//Сохраняем настройки
    }
    //Метод инициализации менеджера
    private void InitializeManager()
    {
        //Здесь мы загружаем и конвертируем настройки из PlayerPrefs
        music = System.Convert.ToBoolean(PlayerPrefs.GetString("music", "true"));
        sounds = System.Convert.ToBoolean(PlayerPrefs.GetString("sounds", "true"));
        SceneManager.sceneLoaded += OnSceneLoaded; //Подписка на событие загрузки сцены
    }

    private void OnDestroy()
    {
        //Отписка от события - что бы не было утечки памяти
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Этот метод будет вызван всякий раз, когда загружается новая сцена
       
        if (_playerAudio != null)
        {
            InitializePlayerAudio(); //Обновление ссылок или состояний
        }
    }
}
