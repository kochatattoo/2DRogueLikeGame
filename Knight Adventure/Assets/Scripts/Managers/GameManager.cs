using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ResourcesLoadManager resourcesLoadManager;
    public SaveManager saveManager; // Ссылка на SaveManager
    public AudioManager audioManager; // Ссылка на AudioManager
    public GUIManager guiManager; // Ссылка на GUIManager
    public GameInput gameInput; // Ссылка на GameInputManager
    public MapManager mapManager; // Ссылка на MapManager
    public PlayerData playerData; // Ссылка на объект PlayerData


    // Методы Awake и Start посмотреть что бы все срабатывали правильно


    private void Awake()
    {
        // Проверка на существование экземпляра GameManager
        if (Instance == null)
        {
            Instance = this; // Установка экземпляра
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены

            SceneManager.sceneLoaded += OnSceneLoaded; //Подписка на событие загрузки сцены
            InitializeManagers();
            InitializeSingletons();

        }
        else
        {
            Destroy(gameObject); // Удаляем второй экземпляр
        }
        // Инициализация ссылок на синглтоны
       // InitializeSingletons();
    }
    private void Start()
    {
        playerData = saveManager.LoadLastGame(); 
    }

    private void OnDestroy()
    {
        //Отписка от события - что бы не было утечки памяти
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Этот метод будет вызван всякий раз, когда загружается новая сцена
        Debug.Log("Scene Loaded^ " + scene.name);
        UpdateSceneReferences(); //Обновление ссылок или состояний
    }
    
    private void UpdateSceneReferences()
    {
        // Обновите ссылки на объекты, если это необходимо
        // Например, если у вас есть ссылки на объекты в определённой сцене
        saveManager = FindObjectOfType<SaveManager>();
        audioManager = FindObjectOfType<AudioManager>();
        guiManager = FindObjectOfType<GUIManager>();
        gameInput = FindObjectOfType<GameInput>();
        mapManager = FindObjectOfType<MapManager>();

    }
    private void InitializeManagers()
    {
        resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>(); // Создаем экземпляр ResourcesLoadManager

        //Поиск необходим элементов в объектах сцены
        saveManager = FindObjectOfType<SaveManager>();
        audioManager = FindObjectOfType<AudioManager>();
        guiManager = FindObjectOfType<GUIManager>();
        gameInput = FindObjectOfType<GameInput>();
        mapManager = FindObjectOfType<MapManager>();


        // Проверяем, найдены ли менеджеры
        Debug.Assert(saveManager != null, "SaveManager not found in the scene."); //Проверка на наличие подключения менеджера
        Debug.Assert(audioManager != null, "AudioManager not found in the scene.");
        Debug.Assert(guiManager != null, "GUIManager not found in the scene.");
        Debug.Assert(gameInput != null, "GameInput not found in the scene");
        Debug.Assert(mapManager != null, "MapManager not found  in the scene");
    }


    //Метод для инициализации ссылок на синглтоны
    private void InitializeSingletons()
    {
        saveManager = SaveManager.Instance; // Получение ссылки на SaveManager
        audioManager = AudioManager.Instance; // Получение ссылки на AudioManager
        guiManager = GUIManager.Instance; // Получение ссылки на GUIManager
        gameInput = GameInput.Instance; // Получение ссылки на GameInput
        mapManager = MapManager.Instance; //Получение ссылки на MapManager
        playerData = PlayerData.Instance; //Получение ссылки на PlayerData
    }


    // Вы можете добавить методы, которые обращаются к функционалу других менеджеров
    public void SaveGame(string fileName)
    {
        saveManager.SaveGame(playerData, fileName);
    }

    // Другие методы для взаимодействия с другими синглтонами...

}