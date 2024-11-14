using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject GameMenu;
    [SerializeField] GameObject StartMenu;
    [SerializeField] GameObject LoadMenu;
    [SerializeField] GameObject OptionMenu;
    [SerializeField] GameObject QuitMenu;


    [SerializeField] TMP_InputField Name;
   private void Awake()
    {
        GameMenu.SetActive(true);
        StartMenu.SetActive(false);
        LoadMenu.SetActive(false);
        OptionMenu.SetActive(false);
        QuitMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void LoadGame()
    {

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
            
        }
        else
            Debug.Log("Слишком короткое Имя, Введите длинее");

    }
  
}
