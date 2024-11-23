using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

[RequireComponent(typeof(GameObject))]
public class PauseMenu : MonoBehaviour
{
    //��������� ��������� ���� �����
    [SerializeField] GameObject _pauseMenuDisplay;

    //���������� ����������� �� �����
    private bool _pauseGame;

    private void Awake()
    {
        _pauseMenuDisplay.SetActive(false);
    }
    private void Start()
    {
        //������������� �� ������� �����
        GameInput.Instance.OnPlayerPause += Player_OnPlayerPause;
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
        _pauseMenuDisplay.SetActive(false);
        Time.timeScale = 1.0f;
        _pauseGame = false;
    }

    public void Pause()
    {
        _pauseMenuDisplay.SetActive(true);
        Time.timeScale = 0f;
        _pauseGame = true;

        //�������� ��������� �����
        //Debug.Log(GameManager.Instance.user.GetName());
    }

    
    //�������� ������ ������ �� ������ � ���������� � Instance
    //� ����������� ������� ���������� � ������� ������ � ��������� ����
    public void SaveGame()
    {
       // GameManager.Instance.user.SaveUserSerialize();
        SaveManager.SaveUser(User.Instance);
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
