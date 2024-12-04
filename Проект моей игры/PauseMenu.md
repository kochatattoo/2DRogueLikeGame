Отдельный скрипт для реализации действий в GUI - во время игры и постановке игру на паузу 
*Перенес из GUIManager часть кода*
```
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

[RequireComponent(typeof(GameObject))]
public class PauseMenu : MonoBehaviour
{
    //Объявляем переменую Меню паузы
    [SerializeField] GameObject _pauseMenuDisplay;

    //Переменная отвеячающая за паузу
    private bool _pauseGame;

    private void Awake()
    {
        _pauseMenuDisplay.SetActive(false);
    }
    private void Start()
    {
        //Подписываемся на событие паузы
        GameInput.Instance.OnPlayerPause += Player_OnPlayerPause;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        _pauseMenuDisplay.SetActive(false);
        Time.timeScale = 1.0f;
        _pauseGame = false;
    }

    public void Pause()
    {
        _pauseMenuDisplay.SetActive(true);
        Time.timeScale = 0f;
        _pauseGame = true;

        //Проверка состояния Юзера
        //Debug.Log(GameManager.Instance.user.GetName());
    }

    
    public void SaveGame()
    {
       // GameManager.Instance.user.SaveUserSerialize();
        SaveManager.SaveUser(GameManager.Instance.user);
    }

    public void LoadGame()
    {
        // GameManager.Instance.user.LoadUserSerialize();
        SaveManager.LoadUser();
        GUIManager.Instance.SetTextAreas();
    }

    public void ResetData()
    {
        // GameManager.Instance.user.ResetData();
        SaveManager.ResetData();
        GUIManager.Instance.SetTextAreas();
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        GameInput.Instance.DisableMovement();
    }
}
```
Данный скрипт нам необходим для внедрения логики в пункт меню паузы 

Большая часть логики осуществляется в UnityДобавил пару методов для возможности загружать объект с префаба, а не прямо с элемента сцены
``` 
 public void OpenWindow()
 {
     if(_pausemenuWindow != null)
     {
         Destroy( _pausemenuWindow );
     }

     _pausemenuWindow = Instantiate(_pauseMenuDisplayPref);
     _pausemenuWindow.transform.SetParent(GameObject.Find("PauseMenu").transform, false );
 }

 public void CloseCurrentWindow()
 {
     if( _pausemenuWindow != null)
     {
         Destroy( _pausemenuWindow );
         _pausemenuWindow = null;
     }
     Destroy(_pausemenuWindow);
 }
```