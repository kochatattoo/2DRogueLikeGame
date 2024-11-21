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


    [SerializeField] TMP_InputField Name;
   private void Awake()
    {
        GameMenu.SetActive(true);
        StartMenu.SetActive(false);
        LoadMenu.SetActive(false);
        OptionMenu.SetActive(false);
        QuitMenu.SetActive(false);
        PanelBarMenu.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void LoadGame()
    {
        if (SaveManager.LoadUser() != null)
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
            GameManager.Instance.user.SetName(InputName);
            GameManager.Instance.user.SetLevel(1);
            GameManager.Instance.user.SetCoins(10);

            SaveManager.SaveUser(GameManager.Instance.user);
            SaveManager.Instance.SaveGame(GameManager.Instance.user, InputName);
            
        }
        else
            Debug.Log("Слишком короткое Имя, Введите длинее");

    }

}
