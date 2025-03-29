using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour,IManager, IAudioManager
{
    public static bool music = true; //Параметр доступности музыки
    public static bool sounds = true; //Параметр доступности звуков

    private AudioSource _audioSource;
    [SerializeField] AudioPlayer _playerAudio;
    
  
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
        if (_audioSource != null)
        {
            _audioSource.mute = !_audioSource.mute;
        }
        else
        {

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
