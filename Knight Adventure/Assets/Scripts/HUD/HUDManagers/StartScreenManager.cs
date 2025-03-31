using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;

public class StartScreenManager : MonoBehaviour, IManager, IStartScreenManager
{
    [Header("UI Elements")]
    private GameObject startScreenPrefab; // Префаб стартового экрана
    private Button continueButton; // Кнопка для продолжения
    private ButtonClickAudio _buttonClickAudio;

    private GameObject startScreen; // экземпляр стартового экрана
    private IGameInput gameInput;
    private IGUIManager guiManager;

    public void StartManager()
    {
        ResourcesLoadManager resourcesLoadManager  = gameObject.AddComponent<ResourcesLoadManager>(); 
        startScreenPrefab = resourcesLoadManager.LoadStartScreenWindow("Star_Screen_Window");
        // startScreenPrefab = Resources.Load<GameObject>("Windows/StartScreenWindow/Star_Screen_Window");
        gameInput = ServiceLocator.GetService<IGameInput>();
        guiManager = ServiceLocator.GetService<IGUIManager>();

        _buttonClickAudio = gameObject.AddComponent<ButtonClickAudio>();
        _buttonClickAudio.StartScript();

        InitializeStartScreen(); // Инициализация стартового экрана
    }
    private void InitializeStartScreen()
    {
        gameInput.DisableMovement();

        // Создание экземпляра стартового экрана
        startScreen = Instantiate(startScreenPrefab);
        startScreen.transform.SetParent(GameObject.Find("GUI_Display").transform, false); // Привязываем к Canvas

        continueButton = FindObjectOfType<Button>();
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

        //var audioManager = ServiceLocator.GetService<IAudioManager>();
        //audioManager.PlayAudio(0);

        _buttonClickAudio.PlayClickAudio();

        // Здесь мы открываем первое окно информации из очереди, если оно есть
        if (guiManager != null) // Проверяем, что GUIManager существует
        {
            guiManager.ShowWindowQueue(); // Открываем первое информационное окно как пример
        }
    }
}
