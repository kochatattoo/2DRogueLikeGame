using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerHelper : MonoBehaviour, IManager
{
    public delegate void SceneLoadedHandler(string sceneName);
    public static event SceneLoadedHandler OnSceneLoaded;

    private void Start()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    public void StartManager()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke(scene.name); // Вызываем событие для подписчиков
    }
}

// В другом классе вы можете подписаться на это событие
public class ExampleListener : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManagerHelper.OnSceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManagerHelper.OnSceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(string sceneName)
    {
        Debug.Log("New scene loaded: " + sceneName);
        // Обновите состояние в зависимости от загруженной сцены
    }
}
