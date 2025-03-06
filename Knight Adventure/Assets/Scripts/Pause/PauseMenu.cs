using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private IGameInput _gameInput;
    private IGUIManager _guiManager;
    private ISaveManager _saveManager;
    private ResourcesLoadManager _resourcesLoadManager;


    public void Restart()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        var pauseButtonAction = FindObjectOfType<PauseButtonAction>();
        pauseButtonAction.HandleStatusPause();

    }
    //И реализовать быстрое сохранение с записью данных о состоянии игры
    public void SaveGame()
    {
        _saveManager=ServiceLocator.GetService<ISaveManager>();
        _saveManager.QuickSaveGame(Player.Instance.playerData);
    }

    public void LoadGame()
    {
        _saveManager = ServiceLocator.GetService<ISaveManager>();
        _guiManager = ServiceLocator.GetService<IGUIManager>();

        _saveManager.LoadLastGame();
        _guiManager.SetTextAreas();
    }

    public void LoadMainMenuScene()
    {
        _gameInput = ServiceLocator.GetService<IGameInput>();
        _gameInput.DisableMovement();

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        
    }
}
