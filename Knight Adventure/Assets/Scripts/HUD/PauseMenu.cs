using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //Переменная отвечающая за паузу
    private bool _pauseGame;

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
      
        GUIManager.Instance.CloseCurrentWindow();
        GameInput.Instance.EnableMovement(); // Включает действия игрока
        Time.timeScale = 1.0f;
        _pauseGame = false;
    }

    public void Pause()
    {
        GUIManager.Instance.OpenPlayerWindow(0);
        GameInput.Instance.DisableMovement(); // Отключает действия игрока, но не дает действия для кнопки ESC
        Time.timeScale = 0f;
        _pauseGame = true;

    }

    //И реализовать быстрое сохранение с записью данных о состоянии игры
    public void SaveGame()
    {
        SaveManager.Instance.QuickSaveGame(GameManager.Instance.playerData);
    }

    public void LoadGame()
    {
        SaveManager.Instance.LoadLastGame();
        GUIManager.Instance.SetTextAreas();
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        GameInput.Instance.DisableMovement();
    }
}
