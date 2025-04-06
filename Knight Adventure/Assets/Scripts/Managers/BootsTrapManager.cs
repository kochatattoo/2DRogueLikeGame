using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapManager : MonoBehaviour
{
  
    private void Awake()
    {
        // Проверяем, что объекты этого класса существуют только в одном экземпляре
        if (FindObjectsOfType<BootsTrapManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Не уничтожать объект при переходе между сценами
        DontDestroyOnLoad(gameObject);

        InitializeGameStateManager();
        InitializeManager();

        // Загрузка начальной сцены
        LoadInitialScene();
       
    }

    private void LoadInitialScene()
    {
        // Загружаем основную сцену (например, "MainMenu")
        SceneManager.LoadScene("Menu");
    }

    private void InitializeGameStateManager()
    {
        // Создаем объект GameStateManager
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
