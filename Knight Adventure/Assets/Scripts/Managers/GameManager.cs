using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameManager
{
    
    public static GameManager Instance { get; private set; }
    public ResourcesLoadManager resourcesLoadManager;
    public SaveManager saveManager; // ������ �� SaveManager
    public AudioManager audioManager; // ������ �� AudioManager
    public GUIManager guiManager; // ������ �� GUIManager
    public GameInput gameInput; // ������ �� GameInputManager
    public MapManager mapManager; // ������ �� MapManager
    public PlayerData playerData; // ������ �� ������ PlayerData


    // ������ Awake � Start ���������� ��� �� ��� ����������� ���������


    private void Awake()
    {
       
    }
    private void Start()
    {
        var saveManagerLocator = ServiceLocator.GetService<ISaveManager>();
        playerData = saveManagerLocator.LoadLastGame();
        resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>();
    }
    public void StartManager()
    {
        var saveManagerLocator = ServiceLocator.GetService<ISaveManager>();
        playerData = saveManagerLocator.LoadLastGame();
    }
    private void OnDestroy()
    {
        //������� �� ������� - ��� �� �� ���� ������ ������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //���� ����� ����� ������ ������ ���, ����� ����������� ����� �����
        Debug.Log("Scene Loaded^ " + scene.name);
        UpdateSceneReferences(); //���������� ������ ��� ���������
    }
    
    private void UpdateSceneReferences()
    {
        // �������� ������ �� �������, ���� ��� ����������
        // ��������, ���� � ��� ���� ������ �� ������� � ����������� �����
        saveManager = FindObjectOfType<SaveManager>();
        audioManager = FindObjectOfType<AudioManager>();
        guiManager = FindObjectOfType<GUIManager>();
        gameInput = FindObjectOfType<GameInput>();
        mapManager = FindObjectOfType<MapManager>();

    }
    private void InitializeManagers()
    {
        resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>(); // ������� ��������� ResourcesLoadManager

        //����� ��������� ��������� � �������� �����
        saveManager = FindObjectOfType<SaveManager>();
        audioManager = FindObjectOfType<AudioManager>();
        guiManager = FindObjectOfType<GUIManager>();
        gameInput = FindObjectOfType<GameInput>();
        mapManager = FindObjectOfType<MapManager>();


        // ���������, ������� �� ���������
        Debug.Assert(saveManager != null, "SaveManager not found in the scene."); //�������� �� ������� ����������� ���������
        Debug.Assert(audioManager != null, "AudioManager not found in the scene.");
        Debug.Assert(guiManager != null, "GUIManager not found in the scene.");
        Debug.Assert(gameInput != null, "GameInput not found in the scene");
        Debug.Assert(mapManager != null, "MapManager not found  in the scene");
    }


    //����� ��� ������������� ������ �� ���������
    private void InitializeSingletons()
    {
        saveManager = SaveManager.Instance; // ��������� ������ �� SaveManager
        guiManager = GUIManager.Instance; // ��������� ������ �� GUIManager
        mapManager = MapManager.Instance; //��������� ������ �� MapManager
    }


    // �� ������ �������� ������, ������� ���������� � ����������� ������ ����������
    public void SaveGame(string fileName)
    {
        saveManager.SaveGame(playerData, fileName);
    }

    // ������ ������ ��� �������������� � ������� �����������...

}