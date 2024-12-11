using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameObject))]
public class PauseMenu : MonoBehaviour
{
    //Объявляем переменую Меню паузы
    public GameObject _pauseMenuDisplayPref;
    private GameObject _pausemenuWindow;

    //Переменная отвеячающая за паузу
    private bool _pauseGame;

    private void Awake()
    {
        // _pauseMenuDisplay.SetActive(false);

        //if(_pauseMenuDisplayPref.gameObject != null)
        //{
        //    Destroy(_pauseMenuDisplayPref.gameObject);
        //}

        // GUIManager.Instance.CloseCurrentWindow();
    }
    private void Start()
    {
        //Подписываемся на событие паузы
        GameInput.Instance.OnPlayerPause += Player_OnPlayerPause;

        GUIManager.Instance.CloseCurrentWindow();
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
       // _pauseMenuDisplay.SetActive(false);
      // Destroy(_pauseMenuDisplayPref.gameObject);

        GUIManager.Instance.CloseCurrentWindow();
        Time.timeScale = 1.0f;
        _pauseGame = false;
    }

    public void Pause()
    {
        // _pauseMenuDisplay.SetActive(true);
        // Instantiate(_pauseMenuDisplayPref);
        // _pauseMenuDisplayPref.transform.SetParent(GameObject.Find("Canvas").transform, false);

        GUIManager.Instance.OpenWindow(0);
        Time.timeScale = 0f;
        _pauseGame = true;

        //Проверка состояния Юзера
        //Debug.Log(GameManager.Instance.user.GetName());
    }

    
    //И реализовать быстрое сохранение с записью данных о состоянии игры
    public void SaveGame()
    {
       // GameManager.Instance.user.SaveUserSerialize();
        SaveManager.Instance.QuickSaveGame(GameManager.Instance.playerData);
    }

    public void LoadGame()
    {
        // GameManager.Instance.user.LoadUserSerialize();
        SaveManager.Instance.LoadLastGame();
        GUIManager.Instance.SetTextAreas();
    }

    //public void ResetData()
    //{
    //    // GameManager.Instance.user.ResetData();
    //    SaveManager.ResetData();
    //    GUIManager.Instance.SetTextAreas();
    //}

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        GameInput.Instance.DisableMovement();
    }

    //public void OpenWindow()
    //{
    //    if(_pausemenuWindow != null)
    //    {
    //        Destroy( _pausemenuWindow );
    //    }

    //    _pausemenuWindow = Instantiate(_pauseMenuDisplayPref);
    //    _pausemenuWindow.transform.SetParent(GameObject.Find("PauseMenu").transform, false );
    //}

    //public void CloseCurrentWindow()
    //{
    //    if( _pausemenuWindow != null)
    //    {
    //        Destroy( _pausemenuWindow );
    //        _pausemenuWindow = null;
    //    }
    //    Destroy(_pausemenuWindow);
    //}
}
