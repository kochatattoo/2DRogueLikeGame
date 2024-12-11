using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public AudioClip openSound;  // Звук появления окна
    public AudioClip closeSound; // Звук закрытия окна
    public Button closeButton;    // Кнопка закрытия окна
    public Button okButton;       // Кнопка "ОК"

    private AudioSource audioSource;  // Аудиомодуль
    private static Window activeWindow; // Текущее активное окно
    private static Queue<Window> windowQueue = new Queue<Window>(); // Очередь окон
    private static bool isPaused = false; // Флаг паузы

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем компонент AudioSource
        closeButton.onClick.AddListener(CloseWindow); // Подписка на событие кнопки закрытия
        okButton.onClick.AddListener(OnOkButtonClicked); // Подписка на событие кнопки "ОК"
    }

    public virtual void OpenWindow()
    {
        PlaySound(openSound); // Воспроизведение звука открытия
        gameObject.SetActive(true); // Активируем окно
        Time.timeScale = 0; // Ставим игру на паузу
        isPaused = true; // Устанавливаем флаг паузы
        activeWindow = this; // Устанавливаем активное окно
    }

    public virtual void CloseWindow()
    {
        if (activeWindow == this) // Проверка, является ли текущее активное окно
        {
            PlaySound(closeSound); // Воспроизведение звука закрытия
            gameObject.SetActive(false); // Деактивируем окно
            Time.timeScale = 1; // Возобновляем игру
            isPaused = false; // Сбрасываем флаг паузы
            activeWindow = null; // Убираем ссылку на активное окно
            ShowNextWindow(); // Показ следующего окна из очереди
        }
    }

    protected virtual void OnOkButtonClicked()
    {
        // Закрываем окно при нажатии "ОК"
        CloseWindow();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public static void QueueWindow(Window window) // Метод для добавления в очередь
    {
        windowQueue.Enqueue(window);
        if (activeWindow == null)
        {
            ShowNextWindow(); // Показать следующее окно в очереди
        }
    }

    public static void ShowNextWindow() // Метод для показа следующего окна
    {
        if (windowQueue.Count > 0)
        {
            Window nextWindow = windowQueue.Dequeue(); // Получаем следующее окно из очереди
            if (nextWindow != null)
            {
                nextWindow.OpenWindow(); // Открываем следующее окно
            }
        }
        else
        {
            Time.timeScale = 1; // Возвращаем игровую скорость к норме, когда нет активных окон
        }
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
