using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Scripts.Interfaces;


public class MainMenuManager : MonoBehaviour, IMainMenuManager
{
    [SerializeField] GameObject GameMenu;
    [SerializeField] GameObject StartMenu;
    [SerializeField] GameObject LoadMenu;
    [SerializeField] GameObject OptionMenu;
    [SerializeField] GameObject QuitMenu;
    [SerializeField] GameObject PanelBarMenu;
    [SerializeField] GameObject User_Name_Panel;
    

    [SerializeField] TMP_Text User_Name;
    [SerializeField] private TMP_InputField Name;

    [SerializeField] private SaveLoadMenu SaveMenu;

    [SerializeField]private GameObject _currentWindow; // ������� ����

    private void Awake()
    {
        GameMenu.SetActive(true);
        StartMenu.SetActive(false);
        LoadMenu.SetActive(false);
        OptionMenu.SetActive(false);
        QuitMenu.SetActive(false);
        //CloseCurrentWindow();
        //OpenGameMenu();
        PanelBarMenu.SetActive(true);
        User_Name_Panel.SetActive(false);
        
    }
    private void Start()
    {
        //if (GameManager.Instance.playerData !=null)
        //{
        //    User_Name_Panel.SetActive(true);
        //    User_Name.text = GameManager.Instance.playerData.name;
        //    Debug.Log(GameManager.Instance.playerData.name + "User name !=null");
        //}

        ////SaveMenu = FindAnyObjectByType<SaveLoadMenu>();
        ////User_Name =FindAnyObjectByType<TMP_Text>();
        //SaveMenu.StartScript();
        //SaveMenu._LoadGame += SaveMenu_Refresh;
    }
    public void StartManager()
    {
        if (GameManager.Instance.playerData != null)
        {
            User_Name_Panel.SetActive(true);
            User_Name.text = GameManager.Instance.playerData.name;
            Debug.Log(GameManager.Instance.playerData.name + "User name !=null");
        }

        //SaveMenu = FindAnyObjectByType<SaveLoadMenu>();
        //User_Name =FindAnyObjectByType<TMP_Text>();
        SaveMenu.StartScript();
        SaveMenu._LoadGame += SaveMenu_Refresh;
    }
    private void SaveMenu_Refresh(object sender, System.EventArgs e)
    {
        RefreshName();
    }
    
    // ������ � ������� ��������� �������� ���� ����������� �������� �������� //////////////////////////////////////
    // �� ���������� � ���������, ������ �� ��� ������ ������� - ������ ����������� � ������� � ����� /////////////
    // � �� ������� �� ����� ������� � ������� �� �������� ������ ������ � ��������///////////////////////////////

    public void OpenMenuWindow(GameObject window)
    {
        // ���������� ������� ����, ���� ��� ����������
        CloseCurrentWindow();
        // �������� ������ ����
        _currentWindow = Instantiate(window);
        // ���������, ��� ����� ���� ����������� � Canvas
        _currentWindow.transform.SetParent(GameObject.Find("Background").transform, false);
        _currentWindow.SetActive(true) ;
        
       
    }
    // ����� ��� �������� �������� ����
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
        OpenMenuWindow(GameMenu);
    }

    public void OpenStartMenu()
    {
        OpenMenuWindow(StartMenu);

        Name = FindObjectOfType<TMP_InputField>();
        Debug.Log(Name);
    }

    public void OpenLoadMenu()
    {
        OpenMenuWindow(LoadMenu);

        SaveMenu = FindObjectOfType<SaveLoadMenu>();
        SaveMenu._LoadGame += SaveMenu_Refresh;

        Debug.Log(SaveMenu);
    }

    public void OpenOptionMenu()
    {
        OpenMenuWindow(OptionMenu);
    }
    public void OpenQuitMenu()
    {
        OpenMenuWindow(QuitMenu);
    }

    ///////////////////////��� �� ����, � ������� �����////////////////////////////////

    public void RefreshName()
    {
        User_Name.text = GameManager.Instance.playerData.name;
        Debug.Log(GameManager.Instance.playerData.name + "User name !=null");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        //������������ ����� �������� ��� �����������
    }

    public void LoadGame()
    {
        if (SaveManager.Instance.LoadLastGame() != null)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.LogError("������������ �� ����������");
        }
    }
    public void Create()
    {
        string InputName=Name.text;
        if (Name.text.Length > 4)
        {

            GameManager.Instance.playerData.name=InputName;

            SetCharacteristics();
            SetRewardsAndAchivements();

            //SaveManager.SaveUser(User.Instance);
            SaveManager.Instance.SaveGame(GameManager.Instance.playerData, InputName);
            
        }
        else
            Debug.Log("������� �������� ���, ������� ������");

    }

    private void SetCharacteristics()
    {
        GameManager.Instance.playerData.playerStats.CreatePlayerCharacteristics(GameManager.Instance.playerData);
    }

    private void SetRewardsAndAchivements()
    {
        GameManager.Instance.playerData.playerAchievements = new Assets.Scripts.Player.PlayerAchievements();
    }
}
