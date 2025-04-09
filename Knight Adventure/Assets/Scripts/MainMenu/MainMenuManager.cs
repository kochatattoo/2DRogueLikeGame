using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;

public class MainMenuManager : MonoBehaviour, IMainMenuManager, IManager
{
    [SerializeField] private GameObject _firstStartMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _loadMenu;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _quitMenu;
    [SerializeField] private GameObject _panelBarMenu;
    [SerializeField] private GameObject _user_Name_Panel;
    [SerializeField] private GameObject _currentWindow; // Òåêóùåå îêíî

    [SerializeField] private TMP_Text _user_Name;
    [SerializeField] private TMP_InputField _nameInputField;

    [SerializeField] private SaveLoadMenu _saveLoadMenu;
    [SerializeField] private GameObject _background;
    private Material _blurMaterial;

    private ISaveManager _saveManager;
    private IAutarizationManager _autarizationManager;
    private PlayerData _playerData;

    private ButtonClickAudio _buttonClickAudio; // TODO: Не помню можно убрать его или нет
  
    public void StartManager()
    { 
  	    ActivateMenu();
		
        GetBlurController();
        BlurOff();

        _saveManager=ServiceLocator.GetService<ISaveManager>();
        _autarizationManager=ServiceLocator.GetService<IAutarizationManager>();
		
		_playerData = _autarizationManager.GetPlayerData(); 
        if (_playerData != null)
        {
            _user_Name.text = _playerData.name;
            _user_Name_Panel.SetActive(true);
            _gameMenu.SetActive(true);
            Debug.Log(_playerData.name + "User name !=null");
        }
        else
        {
            _user_Name_Panel.SetActive(false);
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
    private void SaveMenu_Refresh(object sender, System.EventArgs e)
    {
        RefreshName();
    }
    public void DisableManager()
    {
        _saveLoadMenu._LoadGame -= SaveMenu_Refresh;
    }
    public void RefreshName()
    {
        _user_Name.text = _autarizationManager.GetPlayerData().name;
        _playerData= _autarizationManager.GetPlayerData();
        Debug.Log(_playerData.name + "User name !=null");
    }

    public void StartGame()
    {
        if (_playerData != null)
        {
            BlurOff();
            SceneManager.LoadScene("Game");
        }
        else
        {
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.OpenNotificationWindow("Error", "Player don't create");
            Debug.Log("Игрок не создан");
        }
    }
    public void LoadGame()
    {
        if(_playerData !=null)
        {
            BlurOff();
            SceneManager.LoadScene("Game");
        }
        else
        {
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.OpenNotificationWindow("Error", "Player don't load");
            Debug.Log("Игрок не загружен");
        }
    }
    public void Create()
    {
        do {
            string InputName = _nameInputField.text;
            if (_nameInputField.text.Length > 4)
            {
                _playerData = _playerData.CreatePlayer(InputName);
                _autarizationManager.SetPlayerData(_playerData);
                _saveManager.SaveGame(_playerData, InputName);

            }
            else
            {
                var notificationManager = ServiceLocator.GetService<INotificationManager>();
                notificationManager.OpenNotificationWindow("Error", "Name cann't be less then 4 symbols");
                Debug.Log("Имя должно быть не меньше 4х символов");
            }
        } while (_nameInputField.text.Length < 4);
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
        var audioManager = ServiceLocator.GetService<IAudioManager>();
        audioManager.PlayClick();
    }
    public void ExitApplication()
    {
#if UNITY_EDITOR
        // В редакторе
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // На сборке
            Application.Quit();
#endif
    }
}
