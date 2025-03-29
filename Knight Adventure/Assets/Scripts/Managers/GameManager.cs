using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IManager, IGameManager
{
   
    public SaveManager saveManager; // ������ �� SaveManager
    public AudioManager audioManager; // ������ �� AudioManager
    public GUIManager guiManager; // ������ �� GUIManager
    public GameInput gameInput; // ������ �� GameInputManager
    public MapManager mapManager; // ������ �� MapManager
    public PlayerData playerData; // ������ �� ������ PlayerData


    // ������ Awake � Start ���������� ��� �� ��� ����������� ���������

    private void Start()
    {
        var saveManagerLocator = ServiceLocator.GetService<ISaveManager>();
        playerData = saveManagerLocator.LoadLastGame();
      
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
  

    // �� ������ �������� ������, ������� ���������� � ����������� ������ ����������
    public void SaveGame(string fileName)
    {
        saveManager.SaveGame(playerData, fileName);
    }

    // ������ ������ ��� �������������� � ������� �����������...

}