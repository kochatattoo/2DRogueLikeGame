using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
   
//Автоматически добавляем необходимые компонент
[RequireComponent (typeof(SceneManager))]
//Класс отвечающий за реализацию HUD меню
  public class GUIManager : MonoBehaviour
    {
       public static GUIManager Instance {  get; private set; }
        //Объявляем переменные текстовых полей
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _coins;
        [SerializeField] TextMeshProUGUI _level;

    public GameObject[] uiPrefabs; // Массив префабов для UI окон

    private GameObject _currentWindow; // Текущее окно

    //private User user;

       private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else 
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            //Debug.Log(GameManager.Instance.user.GetName());

       }

    private void Start()
    {
        User.Instance = SaveManager.Instance.LoadLastGame();
        FirstTextAwake();
    }

    public void SetTextAreas()
    {
        //Присваиваем значеие переменных из значения полей USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }

    public void OpenWindow(int windowIndex)
    {
        // Закрывайте текущее окно, если оно существует
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // Проверка на валидность индекса
        if (windowIndex >= 0 && windowIndex < uiPrefabs.Length)
        {
            // Создание нового окна
            _currentWindow = Instantiate(uiPrefabs[windowIndex]);
            // Убедитесь, что новое окно прикреплено к Canvas
            _currentWindow.transform.SetParent(GameObject.Find("PauseMenu").transform, false);
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
        }
    }

    // Метод для закрытия текущего окна
    public void CloseCurrentWindow()
    {
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
            _currentWindow = null;
        }
    }

    private void FirstTextAwake()
    {
        if (User.Instance == null)
            User.Instance = SaveManager.Instance.LoadLastGame();

        //Присваиваем значеие переменных из значения полей USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }
}
