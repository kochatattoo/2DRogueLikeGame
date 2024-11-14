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

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        GameInput.Instance.DisableMovement();
    }
}
