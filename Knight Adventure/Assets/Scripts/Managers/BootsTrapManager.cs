using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapManager : MonoBehaviour
{
	private GameObject initializeManagerObject;
    
    private void Awake()
    {
        if (FindObjectsOfType<BootsTrapManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        InitializeManager();
        LoadInitialScene();
       
    }

    private void LoadInitialScene()
    {
        SceneManager.LoadScene("Menu");
		InitializeGameStateManager();
    }

    private void InitializeGameStateManager()
    {
        // Ñîçäàåì îáúåêò GameStateManager
        GameObject gameStateManager = new GameObject("GameStateManager");
        gameStateManager.AddComponent<GameStateManager>();
		
        DontDestroyOnLoad(gameStateManager);
    }

    private void InitializeManager()
    {
        initializeManagerObject = new GameObject("InitializeManager");
        var initializeManager = initializeManagerObject.AddComponent<InitializationManager>();
        initializeManager.StartManager();

        DontDestroyOnLoad(initializeManager);
    }
}
