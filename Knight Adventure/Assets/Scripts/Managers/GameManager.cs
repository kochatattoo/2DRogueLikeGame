using Assets.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SaveManager saveManager; // Ссылка на SaveManager
    public AudioManager audioManager; // Ссылка на AudioManager
    public GUIManager guiManager; // Ссылка на GUIManager
    public GameInput gameInput; // Ссылка на GameInputManager
    public User user; // Ссылка на объект User

    private void Awake()
    {
        // Проверка на существование экземпляра GameManager
        if (Instance == null)
        {
            Instance = this; // Установка экземпляра
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены
            
        }
        else
        {
            Destroy(gameObject); // Удаляем второй экземпляр
        }
        // Инициализация ссылок на синглтоны
        InitializeSingletons();
    }

    // Метод для инициализации ссылок на синглтоны
    private void InitializeSingletons()
    {
        saveManager = SaveManager.Instance; // Получение ссылки на SaveManager
        audioManager = AudioManager.Instance; // Получение ссылки на AudioManager
        guiManager = GUIManager.Instance; // Получение ссылки на GUIManager
        user = User.Instance; // Получение ссылки на User
        gameInput = GameInput.Instance; // Получение ссылки на GameInput
    }


    // Вы можете добавить методы, которые обращаются к функционалу других менеджеров
    public void SaveGame(string fileName)
    {
        saveManager.SaveGame(user, fileName);
    }

    // Другие методы для взаимодействия с другими синглтонами...

}