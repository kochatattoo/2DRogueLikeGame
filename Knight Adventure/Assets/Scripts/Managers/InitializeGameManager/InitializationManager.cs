using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

public class InitializationManager : MonoBehaviour, IManager
{
    private static InitializationManager Instance { get; set; }
    private ResourcesLoadManager resourcesLoadManager;

    // Использую их для реакторинга кода
    private GameStateManager _stateMachine;
    private Coroutine _coroutine;

    private string _sceneName;

    private void Awake()
    {
        StartManager();
    }
   
    public void StartManager()
    {
        if (Instance == null)
        {
            _stateMachine = new GameStateManager();
           
            Instance = this; // Установка экземпляра
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены

            resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>(); // Создаем экземпляр ResourcesLoadManager
            SceneManager.sceneLoaded += OnSceneLoaded; //Подписка на событие загрузки сцены

            InitializeStates();

            // _coroutine = StartCoroutine(LoadAndInitializeManagers()); // Начинаем вызов корутины

            // CreateAndRegisterManagers();
            // StartGame();
        }
        else
        {
            Destroy(gameObject); // Удаляем второй экземпляр
        }
    }
    private void InitializeStates()
    {
        _stateMachine.ChangeState(new ResourceLoadingState(this));
        _stateMachine.ChangeState(new ManagerCreationState(this));
        _stateMachine.ChangeState(new StartingGameState(this));
    }
    public void CreateAndRegisterManagers()
    {
        GameObject notificationManagerPrefab = resourcesLoadManager.LoadManager("NotificationManager");
        GameObject notificationManagerInstance = Instantiate(notificationManagerPrefab);
        notificationManagerInstance.gameObject.SetActive(false);
        var notifiactionManager = notificationManagerInstance.GetComponent<NotificationManager>();
        ServiceLocator.RegisterService<INotificationManager>(notifiactionManager);
        DontDestroyOnLoad(notificationManagerInstance);

        GameObject audioManagerPrefab = resourcesLoadManager.LoadManager("AudioManager");
        GameObject audioManagerInstance = Instantiate(audioManagerPrefab);
        audioManagerInstance.gameObject.SetActive(false);
        var audioManager = audioManagerInstance.GetComponent<AudioManager>();
        ServiceLocator.RegisterService<IAudioManager>(audioManager);
        DontDestroyOnLoad(audioManagerInstance);

        GameObject saveManagerPrefab = resourcesLoadManager.LoadManager("SaveLoadManager");
        GameObject saveManagerInstance = Instantiate(saveManagerPrefab);
        saveManagerInstance.gameObject.SetActive(false);
        var saveManager = saveManagerInstance.GetComponent<SaveManager>();
        ServiceLocator.RegisterService<ISaveManager>(saveManager);
        DontDestroyOnLoad(saveManagerInstance);

        GameObject autarizationManagerPrefab = resourcesLoadManager.LoadManager("AutarizationManager");
        GameObject autarizationManagerInstance = Instantiate(autarizationManagerPrefab);
        autarizationManagerInstance.gameObject.SetActive(false);
        var autarizationManager = autarizationManagerInstance.GetComponent<AutarizationManager>();
        ServiceLocator.RegisterService<IAutarizationManager>(autarizationManager);
        DontDestroyOnLoad(autarizationManagerInstance);

        GameObject gameInputManagerPrefab = resourcesLoadManager.LoadManager("GameInput");
        GameObject gameInputManagerInstance = Instantiate(gameInputManagerPrefab);
        gameInputManagerInstance.gameObject.SetActive(false);
        var gameInputManager = gameInputManagerInstance.GetComponent<GameInput>();
        ServiceLocator.RegisterService<IGameInput>(gameInputManager);
        DontDestroyOnLoad(gameInputManagerInstance);

        GameObject mapManagerPrefab = resourcesLoadManager.LoadManager("MapManager");
        GameObject mapManagerInstance = Instantiate(mapManagerPrefab);
        mapManagerInstance.gameObject.SetActive(false);
        var mapManager = mapManagerInstance.GetComponent<MapManager>();
        ServiceLocator.RegisterService<IMapManager>(mapManager);
        DontDestroyOnLoad(mapManagerInstance);

        GameObject guiManagerPrefab = resourcesLoadManager.LoadManager("GUI_Manager");
        GameObject guiManagerInstance = Instantiate(guiManagerPrefab);
        guiManagerInstance.gameObject.SetActive(false);
        var guiManager = guiManagerInstance.GetComponent<GUIManager>();
        ServiceLocator.RegisterService<IGUIManager>(guiManager);
        DontDestroyOnLoad(guiManagerInstance);

        GameObject startScreenManagerPrefab = resourcesLoadManager.LoadManager("StartScreenManager");
        GameObject startScreenManagerInstance = Instantiate(startScreenManagerPrefab);
        startScreenManagerInstance.gameObject.SetActive(false);
        var startScreenManager = startScreenManagerInstance.GetComponent<StartScreenManager>();
        ServiceLocator.RegisterService<IStartScreenManager>(startScreenManager);
        DontDestroyOnLoad(startScreenManagerInstance);

        GameObject gameManagerPrefab = resourcesLoadManager.LoadManager("GameManager");
        GameObject gameManagerInstance = Instantiate(gameManagerPrefab);
        gameManagerInstance.gameObject.SetActive(false);
        var gameManager = gameManagerInstance.GetComponent<GameManager>();
        ServiceLocator.RegisterService<IGameManager>(gameManager);
        DontDestroyOnLoad(gameManagerInstance);

    }
  
    public void StartGame() // TODO
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

        HandleSceneChange(_sceneName); // TODO
    }

    // Из за проверок здесь, вызываются ошибки при инициализации менеджеров в необходимом поряде с корутинами
    // Надо изменять на машину состояний и отслеживать в ней
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _sceneName = scene.name;
        HandleSceneChange(scene.name);
    }
    private void HandleSceneChange(string sceneName)
    {
        switch (sceneName)
        {
            case "Menu": //Сцена меню
                _stateMachine.ChangeState(new MainMenuState(this));
                break;

            case "Game": //Сцена игры
                _stateMachine.ChangeState(new PlayState(this));
                break;
            default:
                break;
        }
    }
    public void EnableMenuManagers()
    {
       var menuManager= FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.StartManager();
        }
    }

    public void DisableMenuManagers()
    {
        var menuManager = FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.DisableManager();
        }
    }

    public void EnableGameManagers()
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

    public void DisableGameManagers()
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

  
    ////////////////////////////////////////////////////////////////////////////
    // Где то тут я пытаюсь переписать на асинхронность и машину состояний

    public void LoadResources()
    {
        _coroutine = StartCoroutine(LoadAndInitializeManagers()); // Начинаем вызов корутины
    }
    private IEnumerator LoadAndInitializeManagers()
    {
        yield return LoadManager<INotificationManager, NotificationManager>("NotificationManager");
        yield return LoadManager<IAudioManager, AudioManager>("AudioManager");
        yield return LoadManager<ISaveManager, SaveManager>("SaveLoadManager");
        yield return LoadManager<IAutarizationManager, AutarizationManager>("AutarizationManager");
        yield return LoadManager<IGameInput, GameInput>("GameInput");
        yield return LoadManager<IMapManager, MapManager>("MapManager");
        yield return LoadManager<IGUIManager, GUIManager>("GUI_Manager");
        yield return LoadManager<IStartScreenManager, StartScreenManager>("StartScreenManager");
        yield return LoadManager<IGameManager, GameManager>("GameManager");

        StartGame();
    }
    private IEnumerator LoadManager<TInterface, TImplementation>(string resourceName)
        where TInterface : class
        where TImplementation : MonoBehaviour, TInterface
    {
        // Загрузка префаба асинхронно
        GameObject prefab = resourcesLoadManager.LoadManager(resourceName);
        yield return new WaitUntil(() => prefab != null); // Даём время на загрузку (можно заменить на асинхронный метод)

        GameObject instance = Instantiate(prefab);
        var manager = instance.GetComponent<TImplementation>();

        if (manager != null)
        {
            ServiceLocator.RegisterService<TInterface>(manager);
            Debug.Log("Service " + resourceName + " was loaded");
            DontDestroyOnLoad(instance);
        }
        else
        {
            Debug.LogError($"Failed to get component of type {typeof(TImplementation)} from {resourceName}");
        }
    }
}