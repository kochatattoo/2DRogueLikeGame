using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapManager : MonoBehaviour
{
  
    private void Awake()
    {
        // ���������, ��� ������� ����� ������ ���������� ������ � ����� ����������
        if (FindObjectsOfType<BootsTrapManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // �� ���������� ������ ��� �������� ����� �������
        DontDestroyOnLoad(gameObject);

        InitializeGameStateManager();
        InitializeManager();

        // �������� ��������� �����
        LoadInitialScene();
       
    }

    private void LoadInitialScene()
    {
        // ��������� �������� ����� (��������, "MainMenu")
        SceneManager.LoadScene("Menu");
    }

    private void InitializeGameStateManager()
    {
        // ������� ������ GameStateManager
        GameObject gameStateManager = new GameObject("GameStateManager");
        gameStateManager.AddComponent<GameStateManager>();
        DontDestroyOnLoad(gameStateManager);
    }

    private void InitializeManager()
    {
        GameObject initializeManagerObject = new GameObject("InitializeManager");
        var initializeManager = initializeManagerObject.AddComponent<InitializationManager>();
        initializeManager.StartManager();
    }
}
