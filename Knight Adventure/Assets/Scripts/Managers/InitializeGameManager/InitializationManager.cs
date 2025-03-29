using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationManager : MonoBehaviour, IManager
{
    private ResourcesLoadManager resourcesLoadManager;
    private static InitializationManager Instance { get; set;}

    private void Awake()
    {
        StartManager();
    }

    public void StartManager()
    {
        if (Instance == null)
        {
            Instance = this; // Установка экземпляра
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены

            resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>(); // Создаем экземпляр ResourcesLoadManager
            SceneManager.sceneLoaded += OnSceneLoaded; //Подписка на событие загрузки сцены

            CreateAndRegisterManagers();
            StartGame();
        }
        else
        {
            Destroy(gameObject); // Удаляем второй экземпляр
        }
    }
  
 
    private void CreateAndRegisterManagers()
    {
        GameObject notificationManagerPrefab = resourcesLoadManager.LoadManager("NotificationManager");
        GameObject notificationManagerInstance = Instantiate(notificationManagerPrefab);
        var notifiactionManager = notificationManagerInstance.GetComponent<NotificationManager>();
        ServiceLocator.RegisterService<INotificationManager>(notifiactionManager);
        DontDestroyOnLoad(notificationManagerInstance);

        GameObject audioManagerPrefab = resourcesLoadManager.LoadManager("AudioManager");
        GameObject audioManagerInstance = Instantiate(audioManagerPrefab);
        var audioManager = audioManagerInstance.GetComponent<AudioManager>();
        ServiceLocator.RegisterService<IAudioManager>(audioManager);
        DontDestroyOnLoad(audioManagerInstance);

        GameObject saveManagerPrefab = resourcesLoadManager.LoadManager("SaveLoadManager");
        GameObject saveManagerInstance = Instantiate(saveManagerPrefab);
        var saveManager = saveManagerInstance.GetComponent<SaveManager>();
        ServiceLocator.RegisterService<ISaveManager>(saveManager);
        DontDestroyOnLoad(saveManagerInstance);

        GameObject autarizationManagerPrefab = resourcesLoadManager.LoadManager("AutarizationManager");
        GameObject autarizationManagerInstance = Instantiate(autarizationManagerPrefab);
        var autarizationManager = autarizationManagerInstance.GetComponent<AutarizationManager>();
        ServiceLocator.RegisterService<IAutarizationManager>(autarizationManager);
        DontDestroyOnLoad(autarizationManagerInstance);

        GameObject gameInputManagerPrefab = resourcesLoadManager.LoadManager("GameInput");
        GameObject gameInputManagerInstance = Instantiate(gameInputManagerPrefab);
        var gameInputManager = gameInputManagerInstance.GetComponent<GameInput>();
        ServiceLocator.RegisterService<IGameInput>(gameInputManager);
        DontDestroyOnLoad(gameInputManagerInstance);

        GameObject mapManagerPrefab = resourcesLoadManager.LoadManager("MapManager");
        GameObject mapManagerInstance = Instantiate(mapManagerPrefab);
        var mapManager = mapManagerInstance.GetComponent<MapManager>();
        ServiceLocator.RegisterService<IMapManager>(mapManager);
        DontDestroyOnLoad(mapManagerInstance);

        GameObject guiManagerPrefab = resourcesLoadManager.LoadManager("GUI_Manager");
        GameObject guiManagerInstance = Instantiate(guiManagerPrefab);
        var guiManager = guiManagerInstance.GetComponent<GUIManager>();
        ServiceLocator.RegisterService<IGUIManager>(guiManager);
        DontDestroyOnLoad(guiManagerInstance);

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
    private void StartGame()
    {
        var notificationManager = ServiceLocator.GetService<INotificationManager>();
        notificationManager.StartManager();

        var audioManager = ServiceLocator.GetService<IAudioManager>();
        audioManager.StartManager();

        var saveManager = ServiceLocator.GetService<ISaveManager>();
        saveManager.StartManager();

        var autarizationManager = ServiceLocator.GetService<IAutarizationManager>();
        autarizationManager.StartManager();

        var gameManager = ServiceLocator.GetService<IGameManager>();
        gameManager.StartManager();

        var menuManager = FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.StartManager();
        }
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
            default:
                break;
        }
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
        var menuManager = FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.DisableManager();
        }
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
  
    private void OnDestroy()
    {
        // Отписка от событий
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}