using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; } //Экземпляр объекта

    public static bool music = true; //Параметр доступности музыки
    public static bool sounds = true; //Параметр доступности звуков
    
    private void Start()
    {
        //Теперь проверяем существование экземпляра
        if(Instance == null)
        {
            //Задаем ссылку на экземпляр объекта
            Instance = this;
        }
        else if(Instance == this)
        {
            //Удаляем объект
            Destroy(gameObject);
        }

        //Теперь нам нужно указать, что бы объект не уничтожался
        //При переходе на другую сцену
        DontDestroyOnLoad(gameObject);

        //И запускаем инициализатор
        InitializeManager();
    }

    //Метод инициализации менеджера
    private void InitializeManager()
    {
        //Здесь мы загружаем и конвертируем настройки из PlayerPrefs
        music = System.Convert.ToBoolean(PlayerPrefs.GetString("music", "true"));
        sounds = System.Convert.ToBoolean(PlayerPrefs.GetString("sounds", "true"));
    }
    public static void SaveSettings()
    {
        PlayerPrefs.SetString("music",music.ToString()); //применяем параметры музыки
        PlayerPrefs.SetString("sounds", sounds.ToString());//Применяем параметры звуков
        PlayerPrefs.Save();//Сохраняем настройки
    }
   
}
