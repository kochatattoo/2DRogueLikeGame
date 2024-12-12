using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //���������� ���������� �� �����
    private bool _pauseGame;

    private void Start()
    {
        //������������� �� ������� �����
        GameInput.Instance.OnPlayerPause += Player_OnPlayerPause;

        GUIManager.Instance.CloseCurrentWindow();
    }

   //������� �����
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
        GameInput.Instance.EnableMovement(); // �������� �������� ������
        Time.timeScale = 1.0f;
        _pauseGame = false;
    }

    public void Pause()
    {
        GUIManager.Instance.OpenPlayerWindow(0);
        GameInput.Instance.DisableMovement(); // ��������� �������� ������, �� �� ���� �������� ��� ������ ESC
        Time.timeScale = 0f;
        _pauseGame = true;

    }

    //� ����������� ������� ���������� � ������� ������ � ��������� ����
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
