using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; } //Экземпляр объекта

    public static bool music = true; //Параметр доступности музыки
    public static bool sounds = true; //Параметр доступности звуков

    private AudioSource _audioSource;
    
    private void Awake()
    {
        //Теперь проверяем существование экземпляра
        if(Instance == null)
        {
            //Задаем ссылку на экземпляр объекта
            Instance = this;
            //Теперь нам нужно указать, что бы объект не уничтожался
            //При переходе на другую сцену
            DontDestroyOnLoad(gameObject);
            return;
        }
        else 
        {
            //Удаляем объект
            Destroy(this.gameObject);
        }

        //И запускаем инициализатор
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
    }
}
