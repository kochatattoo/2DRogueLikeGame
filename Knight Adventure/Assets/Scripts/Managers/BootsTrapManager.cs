using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapManager : MonoBehaviour
{
		
    private void Awake()
    {
        // Ïðîâåðÿåì, ÷òî îáúåêòû ýòîãî êëàññà ñóùåñòâóþò òîëüêî â îäíîì ýêçåìïëÿðå
        if (FindObjectsOfType<BootsTrapManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Íå óíè÷òîæàòü îáúåêò ïðè ïåðåõîäå ìåæäó ñöåíàìè
        DontDestroyOnLoad(gameObject);

        InitializeManager();

        // Çàãðóçêà íà÷àëüíîé ñöåíû
        LoadInitialScene();
       
    }

    private void LoadInitialScene()
    {
        // Çàãðóæàåì îñíîâíóþ ñöåíó (íàïðèìåð, "MainMenu")
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
        GameObject initializeManagerObject = new GameObject("InitializeManager");
        var initializeManager = initializeManagerObject.AddComponent<InitializationManager>();
        initializeManager.StartManager();
    }
}
