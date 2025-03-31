using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;

public class Window : MonoBehaviour
{
    public AudioClip openSound;  // Звук появления окна
    public AudioClip closeSound; // Звук закрытия окна

    protected ButtonClickAudio _buttonClickAudio;

    public Button closeButton;    // Кнопка закрытия окна
    public Button okButton;       // Кнопка "ОК"

    private AudioSource _audioSource;  // Аудиомодуль

    private static Window activeWindow; // Текущее активное окно

    // Событие, вызываемое при закрытии окна
    public event Action OnWindowClosed;

    private void Awake()
    {
        GetButtonClickAudioController();

        _audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем компонент AudioSource
        closeButton.onClick.AddListener(CloseWindow); // Подписка на событие кнопки закрытия
        okButton.onClick.AddListener(OnOkButtonClicked); // Подписка на событие кнопки "ОК"
    }

    private void GetButtonClickAudioController() 
    {
        _buttonClickAudio = gameObject.AddComponent<ButtonClickAudio>();
        _buttonClickAudio.StartScript();
    }

    public virtual void OpenWindow()
    {
        PlaySound(openSound); // Воспроизведение звука открытия
        gameObject.SetActive(true); // Активируем окно
        Time.timeScale = 0; // Ставим игру на паузу
        activeWindow = this; // Устанавливаем активное окно

        var gameInput = ServiceLocator.GetService<IGameInput>();
        gameInput.DisableMovement(); // Отключает действия игрока, но не дает действия для кнопки ESC
    }

    // Почему то возвращает действия игрока, и он может ходить после первого закрытого сообщения
    public virtual void CloseWindow()
    {
        if (activeWindow == this) // Проверка, является ли текущее активное окно
        {
            // PlaySound(closeSound); // Воспроизведение звука закрытия
            _buttonClickAudio.PlayClickAudio();
            //  var audioManager = ServiceLocator.GetService<IAudioManager>();
            //  audioManager.PlayAudio(0); // TODO: что то тут надо сделать с выбором и перечислением

            Time.timeScale = 1; // Возобновляем игру

            OnWindowClosed?.Invoke(); // Вызываем событие закрытия

            if (GUIManager.IsQueueEmpty())
            {
                var gameInput = ServiceLocator.GetService<IGameInput>();
                gameInput.EnableMovement();
            }
            else
            {
                GUIManager.ShowNextWindow(); // Показ следующего окна из очереди
            }
        }

           Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
           var audioManager = ServiceLocator.GetService<IAudioManager>();
           audioManager.PlayAudio(0); // TODO: что то тут надо сделать с выбором и перечислением
    }
    protected virtual void OnOkButtonClicked()
    {
        _buttonClickAudio.PlayClickAudio();
        // Закрываем окно при нажатии "ОК"
        CloseWindow();
    }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    public static void QueueWindow(Window window) // Метод для добавления в очередь
    {
       GUIManager.QueueWindow(window);
    }

    public static void ShowPriorityWindow(Window window) // Метод для показа приоритетных окон
    {
        if (activeWindow != null)
        {
            activeWindow.CloseWindow(); // Закройте текущее активное окно
        }
        window.OpenWindow(); // Откроем приоритетное окно
    }
}
