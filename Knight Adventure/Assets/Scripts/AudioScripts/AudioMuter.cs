using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMuter : MonoBehaviour
{
    //Публичные параметры
    public bool is_music = false;

    //Приватные параметры
    private AudioSource _as; //Audio Source
    private float base_volume = 1F; //Базовая громкость

    void Start()
    {
        //Инициализация компонента при старте игры
        //Получаем компонент AS
        _as=this.gameObject.GetComponent<AudioSource>();
        //Получаем базовую громкость из AS
        base_volume = _as.volume;
    }

    //Каждый кадр мы проверяем параметры и устанавливаем громкость
    void Update()
    {
        //Для начала проверки, музыка это или нет
         if(is_music)
        {
            _as.volume = (AudioManager.music) ? base_volume : 0F;
        }
        else
        {
            _as.volume = (AudioManager.sounds) ? base_volume : 0F;
        }
    }
}
