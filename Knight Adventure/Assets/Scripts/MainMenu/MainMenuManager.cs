using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;


public class MainMenuManager : MonoBehaviour, IMainMenuManager
{
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _loadMenu;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _quitMenu;
    [SerializeField] private GameObject _panelBarMenu;
    [SerializeField] private GameObject _user_Name_Panel;
    [SerializeField] private GameObject _currentWindow; // Текущее окно

    [SerializeField] private TMP_Text _user_Name;
    [SerializeField] private TMP_InputField _name;

    [SerializeField] private SaveLoadMenu _saveLoadMenu;

    private ISaveManager _saveManager;
    private PlayerData _playerData;

    private void Awake()
    {
        _gameMenu.SetActive(true);
        _startMenu.SetActive(false);
        _loadMenu.SetActive(false);
        _optionMenu.SetActive(false);
        _quitMenu.SetActive(false);
        //CloseCurrentWindow();
        //OpenGameMenu();
        _panelBarMenu.SetActive(true);
        _user_Name_Panel.SetActive(false);
        
    }
    private void Start()
    {
     
    }
    public void StartManager()
    {
       _saveManager=ServiceLocator.GetService<ISaveManager>();
        _playerData = _saveManager.LoadLastGame();
        if (_playerData != null)
        {
            _user_Name_Panel.SetActive(true);
            _user_Name.text = _playerData.name;
            Debug.Log(_playerData.name + "User name !=null");
        }
        else
        {
            _user_Name_Panel.SetActive(true);
        }

        _saveLoadMenu.StartScript();
        _saveLoadMenu._LoadGame += SaveMenu_Refresh;
    }
    private void SaveMenu_Refresh(object sender, System.EventArgs e)
    {
        RefreshName();
    }
    
    // Отсюда я пытался настроить загрузку окон посредством загрузки префабов //////////////////////////////////////
    // Но столкнулся с проблемой, почему то при поиске объекта - объект подключался к префабу в папке /////////////
    // А не объекту на сцене Иеархии и поэтому не работали методы записи и загрузки///////////////////////////////

    public void OpenMenuWindow(GameObject window)
    {
        // Закрывайте текущее окно, если оно существует
        CloseCurrentWindow();
        // Создание нового окна
        _currentWindow = Instantiate(window);
        // Убедитесь, что новое окно прикреплено к Canvas
        _currentWindow.transform.SetParent(GameObject.Find("Background").transform, false);
        _currentWindow.SetActive(true) ;
        
       
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

    public void OpenGameMenu()
    {
        OpenMenuWindow(_gameMenu);
    }

    public void OpenStartMenu()
    {
        OpenMenuWindow(_startMenu);

        _name = FindObjectOfType<TMP_InputField>();
        Debug.Log(_name);
    }

    public void OpenLoadMenu()
    {
        OpenMenuWindow(_loadMenu);

        _saveLoadMenu = FindObjectOfType<SaveLoadMenu>();
        _saveLoadMenu._LoadGame += SaveMenu_Refresh;

        Debug.Log(_saveLoadMenu);
    }

    public void OpenOptionMenu()
    {
        OpenMenuWindow(_optionMenu);
    }
    public void OpenQuitMenu()
    {
        OpenMenuWindow(_quitMenu);
    }

    ///////////////////////Вот по сюда, я страдал дичью////////////////////////////////

    public void RefreshName()
    {
        _user_Name.text = _playerData.name;
        Debug.Log(_playerData.name + "User name !=null");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        //Использовать поиск скриптов для подключения
    }

    public void LoadGame()
    {
        if (SaveManager.Instance.LoadLastGame() != null)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.LogError("Пользователь не существует");
        }
    }
    public void Create()
    {
        string InputName=_name.text;
        if (_name.text.Length > 4)
        {

            _playerData.name=InputName;

            SetCharacteristics();
            SetRewardsAndAchivements();

            //SaveManager.SaveUser(User.Instance);
            _saveManager.SaveGame(_playerData, InputName);
            
        }
        else
            Debug.Log("Слишком короткое Имя, Введите длинее");

    }

    private void SetCharacteristics()
    {
        _playerData.playerStats.CreatePlayerCharacteristics(_playerData);
    }

    private void SetRewardsAndAchivements()
    {
        _playerData.playerAchievements = new Assets.Scripts.Player.PlayerAchievements();
    }
}
