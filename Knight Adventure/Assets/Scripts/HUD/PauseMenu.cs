using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //���������� ���������� �� �����
    private bool _pauseGame;
    private IGameInput _gameInput;
    private IGUIManager _guiManager;

    private void Start()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        _guiManager = ServiceLocator.GetService<IGUIManager>();
        //������������� �� ������� �����
        _gameInput.OnPlayerPause += Player_OnPlayerPause;

        _guiManager.CloseCurrentWindow();
    }
    private void OnDestroy()
    {
        _gameInput.OnPlayerPause -= Player_OnPlayerPause;
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
        SceneManager.LoadScene("Game");
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        _guiManager = ServiceLocator.GetService<IGUIManager>();

        _guiManager.CloseCurrentWindow();
        _gameInput.EnableMovement(); // �������� �������� ������
        Time.timeScale = 1.0f;
        _pauseGame = false;
    }

    public void Pause()
    {
        //GUIManager.Instance.OpenPlayerWindow(0);
        _guiManager.OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("PauseMenuDisplay"));
        _gameInput.DisableMovement(); // ��������� �������� ������, �� �� ���� �������� ��� ������ ESC
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
        _guiManager.SetTextAreas();
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        _gameInput.DisableMovement();
    }
}
