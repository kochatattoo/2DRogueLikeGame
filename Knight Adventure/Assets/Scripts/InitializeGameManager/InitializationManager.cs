using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationManager : MonoBehaviour
{
    private IAudioManager audioManager;
    private ISaveManager saveManager;
    private ResourcesLoadManager resourcesLoadManager;

    private void Awake()
    {

        CreateAndRegisterManagers();
        // Инициализация сервисов
        InitializeServices();
        // Выполнение инициализации всех систем
        InitializeSystems();
        // Подписка на события загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(this);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleSceneChange(scene.name);
    }
    private void HandleSceneChange(string sceneName)
    {
        switch (sceneName)
        {
            case "Menu": //Сцена меню
                EnableMenuManagers();
                DisableGameManagers();
                break;

            case "Game": //Сцена игры
                DisableMenuManagers();
                EnableGameManagers();
                break;

            // Добавьте дополнительные случаи для других сцен при необходимости

            default:
                break;
        }
    }
    private void InitializeServices()
    {
       // CreateAndRegisterManagers();

       // var audioManager = FindObjectOfType<AudioManager>();
       // var saveManager = FindObjectOfType<SaveManager>();

        // Регистрация сервисов
       // ServiceLocator.RegisterService<IAudioManager>(audioManager);
      //  ServiceLocator.RegisterService<ISaveManager>(saveManager);

        
    }
    private void InitializeSystems()
    {
        
        // Здесь можно вызывать инициализацию других систем
        // Последовательность можно настроить, чтобы избежать проблем с зависимостямиaaaaaaaaa
        StartGame();
    }
    private void CreateAndRegisterManagers()
    {
        resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>(); // Создаем экземпляр ResourcesLoadManager

        GameObject audioManagerPrefab = resourcesLoadManager.LoadManager("AudioManager");
        GameObject audioManagerInstance = Instantiate(audioManagerPrefab);
        var audioManager = audioManagerInstance.GetComponent<AudioManager>();
        ServiceLocator.RegisterService<IAudioManager>(audioManager);
        DontDestroyOnLoad(audioManagerInstance);

        GameObject gameInputManagerPrefab = resourcesLoadManager.LoadManager("GameInput");
        GameObject gameInputManagerInstance = Instantiate(gameInputManagerPrefab);
        var gameInputManager = gameInputManagerInstance.GetComponent<GameInput>();
        ServiceLocator.RegisterService<IGameInput>(gameInputManager);
        DontDestroyOnLoad(gameInputManagerInstance);

        GameObject guiManagerPrefab = resourcesLoadManager.LoadManager("GUI_Manager");
        GameObject guiManagerInstance = Instantiate(guiManagerPrefab);
        var guiManager = guiManagerInstance.GetComponent<GUIManager>();
        ServiceLocator.RegisterService<IGUIManager>(guiManager);
        DontDestroyOnLoad(guiManagerInstance);

        GameObject mapManagerPrefab = resourcesLoadManager.LoadManager("MapManager");
        GameObject mapManagerInstance = Instantiate(mapManagerPrefab);
        var mapManager = mapManagerInstance.GetComponent<MapManager>();
        ServiceLocator.RegisterService<IMapManager>(mapManager);
        DontDestroyOnLoad(mapManagerInstance);

        //GameObject menuManagerPrefab = resourcesLoadManager.LoadManager("MenuManager");
        //GameObject menuManagerInstance = Instantiate(menuManagerPrefab);
        //var mainMenuManager = FindObjectOfType<MainMenuManager>();
        //ServiceLocator.RegisterService<IMainMenuManager>(mainMenuManager);
        //DontDestroyOnLoad(menuManagerInstance);

        GameObject saveManagerPrefab = resourcesLoadManager.LoadManager("SaveLoadManager");
        GameObject saveManagerInstance = Instantiate(saveManagerPrefab);
        var saveManager = saveManagerInstance.GetComponent<SaveManager>();
        ServiceLocator.RegisterService<ISaveManager>(saveManager);
        DontDestroyOnLoad(saveManagerInstance);

        GameObject startScreenManagerPrefab = resourcesLoadManager.LoadManager("StartScreenManager");
        GameObject startScreenManagerInstance = Instantiate(startScreenManagerPrefab);
        var startScreenManager = startScreenManagerInstance.GetComponent<StartScreenManager>();
        ServiceLocator.RegisterService<IStartScreenManager>(startScreenManager);
        DontDestroyOnLoad(startScreenManagerInstance);

        GameObject gameManagerPrefab = resourcesLoadManager.LoadManager("GameManager");
        GameObject gameManagerInstance = Instantiate(gameManagerPrefab);
        var gameManager = gameManagerInstance.GetComponent<GameManager>();
        ServiceLocator.RegisterService<IGameManager>(gameManager);
        DontDestroyOnLoad(gameManagerInstance);

    }

    private void EnableMenuManagers()
    {
       var menuManager= FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.StartManager();
        }
    }

    private void DisableMenuManagers()
    {

    }

    private void EnableGameManagers()
    {
        // Включаем игровые менеджеры
        var gameInputManager = ServiceLocator.GetService<IGameInput>();
        var mapManager = ServiceLocator.GetService<IMapManager>();
        var guiManager = ServiceLocator.GetService<IGUIManager>();
        var startScreenManager = ServiceLocator.GetService<IStartScreenManager>();

        if (gameInputManager is MonoBehaviour gameInputScript) gameInputScript.gameObject.SetActive(true);
        if (mapManager is MonoBehaviour mapScript) mapScript.gameObject.SetActive(true);
        if (guiManager is MonoBehaviour guiScript) guiScript.gameObject.SetActive(true);
        if (startScreenManager is MonoBehaviour startScreenScript) startScreenScript.gameObject.SetActive(true);

        gameInputManager.StartManager();
        mapManager.StartManager();
        guiManager.StartManager();
        startScreenManager.StartManager();

        var audioManager = ServiceLocator.GetService<IAudioManager>();
        audioManager.InitializePlayerAudio();
    }

    private void DisableGameManagers()
    {
        // Отключаем игровые менеджеры
        var gameInputManager = ServiceLocator.GetService<IGameInput>();
        var mapManager = ServiceLocator.GetService<IMapManager>();
        var guiManager = ServiceLocator.GetService<IGUIManager>();
        var startScreenManager = ServiceLocator.GetService<IStartScreenManager>();
        
        if (gameInputManager is MonoBehaviour gameInputScript) gameInputScript.gameObject.SetActive(false);
        if (mapManager is MonoBehaviour mapScript) mapScript.gameObject.SetActive(false);
        if (guiManager is MonoBehaviour guiScript) guiScript.gameObject.SetActive(false);
        if (startScreenManager is MonoBehaviour startScreenScript) startScreenScript.gameObject.SetActive(false);
    }
    private void StartGame()
    {
        var menuManager = FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.StartManager();
        }

        var audioService = ServiceLocator.GetService<IAudioManager>();
        audioService.StartManager();

        var saveService = ServiceLocator.GetService<ISaveManager>();
        saveService.StartManager();

    }
    private void OnDestroy()
    {
        // Отписка от событий
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}