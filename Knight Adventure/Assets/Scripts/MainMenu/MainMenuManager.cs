using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using Unity.VisualScripting;


public class MainMenuManager : MonoBehaviour, IMainMenuManager
{
    [SerializeField] private GameObject _firstStartMenu;
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
    [SerializeField] private GameObject _background;
    private Material _blurMaterial;

    private ISaveManager _saveManager;
    private IAutarizationManager _autarizationManager;
    private PlayerData _playerData;

   private ButtonClickAudio _buttonClickAudio;
  
    public void StartManager()
    {
        ActivateMenu();

        GetBlurController();
        GetButtonClickAudioController();
        BlurOff();

        _saveManager=ServiceLocator.GetService<ISaveManager>();
        _autarizationManager=ServiceLocator.GetService<IAutarizationManager>();

        _playerData = _autarizationManager.GetPlayerData();
        if (_playerData != null)
        {
            _user_Name_Panel.SetActive(true);
            _user_Name.text = _playerData.name;
            _gameMenu.SetActive(true);
            Debug.Log(_playerData.name + "User name !=null");
        }
        else
        {
            _user_Name_Panel.SetActive(true);
            _firstStartMenu.SetActive(true);
        }

        _saveLoadMenu.StartScript();
        _saveLoadMenu._LoadGame += SaveMenu_Refresh;
    }
    private void ActivateMenu()
    {
        _firstStartMenu.SetActive(false);
        _gameMenu.SetActive(false);
        _startMenu.SetActive(false);
        _loadMenu.SetActive(false);
        _optionMenu.SetActive(false);
        _quitMenu.SetActive(false);
        //CloseCurrentWindow();
        //OpenGameMenu();
        _panelBarMenu.SetActive(true);
        _user_Name_Panel.SetActive(false);
    }
    private void GetBlurController()
    {
        var backgroundImage = _background.GetComponent<Image>();

        if(backgroundImage != null)
        {
            _blurMaterial=backgroundImage.material;
        }
    }
    private void GetButtonClickAudioController()
    {
        _buttonClickAudio = gameObject.AddComponent<ButtonClickAudio>();
        _buttonClickAudio.StartScript();
    }
    private void SaveMenu_Refresh(object sender, System.EventArgs e)
    {
        RefreshName();
    }
    public void DisableManager()
    {
        _saveLoadMenu._LoadGame -= SaveMenu_Refresh;
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
        _user_Name.text = _autarizationManager.GetPlayerData().name;
        _playerData= _autarizationManager.GetPlayerData();
        Debug.Log(_playerData.name + "User name !=null");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        //Использовать поиск скриптов для подключения
    }

    public void LoadGame()
    {

        //if (_saveManager.LoadLastGame() != null)
        if(_playerData !=null)
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
            _autarizationManager.SetPlayerData(_playerData);

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

    public void BlurOn()
    {
        if (_blurMaterial != null)
        {
            _blurMaterial.SetFloat("_BlurAmount", 4.8f);
        }
    }
    public void BlurOff()
    {
        if (_blurMaterial != null)
        {
            _blurMaterial.SetFloat("_BlurAmount", 0.63f);
        }
    }

    public void PlayClickAudio()
    {
        _buttonClickAudio.PlayClickAudio();
    }
}
