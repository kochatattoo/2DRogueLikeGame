Пора добавить в игру немного звукового сопровождения 
Для реализации звука используем знания из [[Работа с аудио]]

Для начала найдем материалы для звукового сопровождения:
- Звук
- Музыка
- Озвучка
И закинем их в файлы проекта для дальнейшего использования 

-> Создаем объект *Create* ->*New Object* -> *AudioSource* -> и присваиваем ему скрипт *AudioManager*

В своем случае написал следующий скрипт
```
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
```

Создал кнопку в меню управлении для включения и отключения звука -> Присвоил объект для действия на кнопке *AudioManager* для срабатывания метода *SoundOffOn*

Такую же кнопку создал в меню паузы в сцене игры и присвоил объект со следующим скриптом 
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMuter : MonoBehaviour
{
    private AudioSource _musicAudioSource;
    
    private void Start()
    {
        _musicAudioSource = GetComponent<AudioSource>();
    }

    public void MuteSound()
    {
        AudioManager.Instance.SoundOffOn();
    }
}
```

К персонажу добавил следующий объект *PlayerAudio* и скрипт
```
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
```

Так же написал часть в скрипте про изменение Изображения с MutOn на MutOff
В *AudioMuter*
```
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
```


## Было принято решение переписать скрипты менеджеров под более гибкие и актуальные

### Шаг 1: Создание интерфейса для AudioManager

Сначала создадим интерфейс `IAudioManager`, который будет определять функционал, который должен быть реализован в классе `AudioManager`.
```
public interface IAudioManager
{
    bool StatusMusic();
    void SoundOffOn();
    void SetVolume(float volume);
    static void SaveSettings(); // Можно оставить как статический метод, если нужно
}
```
