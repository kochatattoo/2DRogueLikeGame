using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Scripts;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject GameMenu;
    [SerializeField] GameObject StartMenu;
    [SerializeField] GameObject LoadMenu;
    [SerializeField] GameObject OptionMenu;
    [SerializeField] GameObject QuitMenu;
    [SerializeField] GameObject PanelBarMenu;
    [SerializeField] GameObject User_Name_Panel;
    

    [SerializeField] TMP_Text User_Name;
    [SerializeField] TMP_InputField Name;
   private void Awake()
    {
        GameMenu.SetActive(true);
        StartMenu.SetActive(false);
        LoadMenu.SetActive(false);
        OptionMenu.SetActive(false);
        QuitMenu.SetActive(false);
        PanelBarMenu.SetActive(true);
        User_Name_Panel.SetActive(false);
        
    }
    private void Start()
    {
        if (User.Instance!=null)
        {
            User_Name_Panel.SetActive(true);
            User_Name.text = User.Instance.name;
            Debug.Log(User.Instance.name + "User name !=null");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void LoadGame()
    {
        if (SaveManager.Instance.LoadLastGame() != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.LogError("Пользователь не существует");
        }

    }
 
    public void Create()
    {
        string InputName=Name.text;
        if (Name.text.Length > 4)
        {
            User.Instance.SetName(InputName);
            User.Instance.SetLevel(1);
            User.Instance.SetCoins(10);

            SaveManager.SaveUser(User.Instance);
            SaveManager.Instance.SaveGame(User.Instance, InputName);
            
        }
        else
            Debug.Log("Слишком короткое Имя, Введите длинее");

    }

}
