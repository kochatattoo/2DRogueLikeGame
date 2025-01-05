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
    }

    private void InitializeServices()
    {
        CreateAndRegisterManagers();

       // var audioManager = FindObjectOfType<AudioManager>();
       // var saveManager = FindObjectOfType<SaveManager>();

        // Регистрация сервисов
       // ServiceLocator.RegisterService<IAudioManager>(audioManager);
      //  ServiceLocator.RegisterService<ISaveManager>(saveManager);

        // Подписка на события загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeSystems()
    {
        
        // Здесь можно вызывать инициализацию других систем
        // Последовательность можно настроить, чтобы избежать проблем с зависимостями
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

        //GameObject gameInputManagerPrefab = resourcesLoadManager.LoadManager("GameInput");
        //GameObject gameInputManagerInstance = Instantiate(gameInputManagerPrefab);
        //var gameInputManager = gameInputManagerInstance.GetComponent<GameInput>();
        //ServiceLocator.RegisterService<IGameInput>(gameInputManager);
        //DontDestroyOnLoad(gameInputManagerInstance);

        //GameObject guiManagerPrefab = resourcesLoadManager.LoadManager("GUI_Manager");
        //GameObject guiManagerInstance = Instantiate(guiManagerPrefab);
        //var guiManager = guiManagerInstance.GetComponent<GUIManager>();
        //ServiceLocator.RegisterService<IGUIManager>(guiManager);
        //DontDestroyOnLoad(guiManagerInstance);

        //GameObject mapManagerPrefab = resourcesLoadManager.LoadManager("MapManager");
        //GameObject mapManagerInstance = Instantiate(mapManagerPrefab);
        //var mapManager = mapManagerInstance.GetComponent<MapManager>();
        //ServiceLocator.RegisterService<IMapManager>(mapManager);
        //DontDestroyOnLoad(mapManagerInstance);

        ////GameObject menuManagerPrefab = resourcesLoadManager.LoadManager("MenuManager");
        ////GameObject menuManagerInstance = Instantiate(menuManagerPrefab);
        ////DontDestroyOnLoad(menuManagerInstance);

        GameObject saveManagerPrefab = resourcesLoadManager.LoadManager("SaveLoadManager");
        GameObject saveManagerInstance = Instantiate(saveManagerPrefab);

        var saveManager = saveManagerInstance.GetComponent<SaveManager>();
        ServiceLocator.RegisterService<ISaveManager>(saveManager);

        DontDestroyOnLoad(saveManagerInstance);

        ////GameObject startScreenManagerPrefab = resourcesLoadManager.LoadManager("StartScreenManager");
        ////GameObject startScreenManagerInstance = Instantiate(startScreenManagerPrefab);
        ////DontDestroyOnLoad(startScreenManagerInstance);

        GameObject gameManagerPrefab = resourcesLoadManager.LoadManager("GameManager");
        GameObject gameManagerInstance = Instantiate(gameManagerPrefab);
        var gameManager = gameManagerInstance.GetComponent<GameManager>();
        ServiceLocator.RegisterService<IGameManager>(gameManager);
        DontDestroyOnLoad(gameManagerInstance);

    }
    private void CreateManagerFromPrefab(string name)
    {
        GameObject ManagerPrefab = resourcesLoadManager.LoadManager(name);
        GameObject ManagerInstance = Instantiate(ManagerPrefab);
    }
    private void StartGame()
    {
        var audioService = ServiceLocator.GetService<IAudioManager>();
        //audioService.PlaySound("game_start");

        var saveService = ServiceLocator.GetService<ISaveManager>();
        //saveService.SaveGame("initial_savefile");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Логика, которая должна выполняться при загрузке сцены
        Debug.Log($"Scene Loaded: {scene.name}");
    }

    private void OnDestroy()
    {
        // Отписка от событий
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}