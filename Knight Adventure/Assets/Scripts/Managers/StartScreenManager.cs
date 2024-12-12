using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject startScreenPrefab; // Префаб стартового экрана
    [SerializeField] private TextMeshProUGUI welcomeText; // Приветственное сообщение
    [SerializeField] private Button continueButton; // Кнопка для продолжения

    private GameObject startScreen; // экземпляр стартового экрана

    private void Start()
    {
        InitializeStartScreen(); // Инициализация стартового экрана
    }

    private void InitializeStartScreen()
    {
        // Создание экземпляра стартового экрана
        startScreen = Instantiate(startScreenPrefab);
        startScreen.transform.SetParent(GameObject.Find("GUI_Display").transform, false); // Привязываем к Canvas

        // Устанавливаем текст приветствия
        welcomeText = startScreen.GetComponentInChildren<TextMeshProUGUI>();
        if (welcomeText != null)
        {
            welcomeText.text = "Добро пожаловать в нашу игру!"; // Установите текст приветствия
        }

        // Привязываем кнопку для продолжения
        continueButton = startScreen.GetComponentInChildren<Button>();
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked); // Подписка на событие кнопки
        }
    }

    private void OnContinueButtonClicked()
    {
        // Скрываем стартовый экран
        Destroy(startScreen);

        // Здесь мы открываем первое окно информации из очереди, если оно есть
        if (GUIManager.Instance != null) // Проверяем, что GUIManager существует
        {
            GUIManager.Instance.OpenInformationWindow(0); // Открываем первое информационное окно как пример
        }
    }
}
