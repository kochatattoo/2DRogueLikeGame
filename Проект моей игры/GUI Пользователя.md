#Unity
Реализация GUI или HUD интерфейса происходит с помощью UI элементов

В своих проекта в Unity я использую следующий способ.
-Добавляем в графу Hierarchy UI-элемент Canvas
-В данный Canvas добавляем пустой элемент - назовем его GUI_Manager
-Создаем скрипт GUIManager и добавляем его нашему новому объекту 
*Пример скрипта*
```
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Scripts
{
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

        User user =new User();

        private void Awake()
        {
              if (Instance == null)
		   {

				   Instance = this;
		   }

			   else if (Instance == this)
		   {
			   Destroy(gameObject);
			}
            SetTextAreas();
            //Debug.Log(GameManager.Instance.user.GetName());

        }
   
        private void SetTextAreas()
        {
            //Присваиваем значеие переменных из значения полей USER
            _name.text = GameManager.Instance.user.GetName();
            _coins.text = GameManager.Instance.user.GetCoins().ToString();
            _level.text = GameManager.Instance.user.GetLevel().ToString();

        }

    }
}
```
Данный скрипт, вытягивает данные о пользователе из класса GameManager где мы будем хранить все необходимые настройки игры.
На данный момент GUIManager отвечает за отображение Имени, Уровня и Количества монет нашего Пользователя.


Так же отдельный скриптом сделал файл реализации пункта меню
```
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameObject))]
public class PauseMenu : MonoBehaviour
{
    //Объявляем переменую Меню паузы
    [SerializeField] GameObject _pauseMenuDisplay;

    //Переменная отвеячающая за паузу
    private bool _pauseGame;

    private void Awake()
    {
       //Закрываем Меню паузы в начале игры
        _pauseMenuDisplay.SetActive(false);
    }
    private void Start()
    {
        //Подписываемся на событие паузы
        GameInput.Instance._OnPlayerPause += Player_OnPlayerPause;
    }

   //Событие паузы
    private void Player_OnPlayerPause(object sender, System.EventArgs e)
    {
        if (_pauseGame)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Restart()
    {
         //Перезагрузка сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Установка стандартного течения времени
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
       //Продолжение игры
       //Убираем меню паузы, возвращаем время
		_pauseMenuDisplay.SetActive(false);
        Time.timeScale = 1.0f;
        _pauseGame = false;
    }

    public void Pause()
    {
		//Пауза игры
		//Ставим меню паузы, время замораживаем     
		_pauseMenuDisplay.SetActive(true);
        Time.timeScale = 0f;
        _pauseGame = true;

        //Проверка состояния Юзера
        //Debug.Log(GameManager.Instance.user.GetName());
    }

    public void LoadMainMenuScene()
    {
	    //Ставим время в стандарт
	    //Загружаем сцену меню
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
```

Большую часть логики можно реализовать прямо в меню Unity, придавая кнопкам действия закрытия и открытия определенных меню
Реализацию [[PauseMenu]] вынес в отдельную заметку

При добавлении новой ФИЧИ-Save&Load код GUIManager терпит следующий изменения
```
  private void Awake()
   {
       Instance = this;
       FirstTextAwake();
      //Debug.Log(GameManager.Instance.user.GetName());

   }
  private void FirstTextAwake()
  {
   user.LoadUserSerialize();
   //Присваиваем значеие переменных из значения полей USER
   _name.text = user.GetName();
   _coins.text = user.GetCoins().ToString();
   _level.text = user.GetLevel().ToString();
   }

  public void SetTextAreas()
   {
       //Присваиваем значеие переменных из значения полей USER
       _name.text = user.GetName();
       _coins.text = user.GetCoins().ToString();
       _level.text = user.GetLevel().ToString();
   }
```

### 1.12.24
Скрипты прошли определыне изменения, и теперь скрипт GUIManager выглядит следующим образом
```
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
```
