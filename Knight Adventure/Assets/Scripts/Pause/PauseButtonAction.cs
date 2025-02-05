using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButtonAction : MonoBehaviour
{
    private bool _pauseGame;
    private void Start()
    {
        var gameInput=ServiceLocator.GetService<IGameInput>();
        gameInput.OnPlayerPause += OnPlayerPause;
    }
    private void OnDisable()
    {
        var gameInput = ServiceLocator.GetService<IGameInput>();
        gameInput.OnPlayerPause -= OnPlayerPause;
    }

    private void OnPlayerPause(object sender, System.EventArgs e)
    {
        HandleStatusPause();
    }
    public void HandleStatusPause()
    {
        if (!_pauseGame)
        {
            _pauseGame = true;
            OpenPauseMenu();

            var gameInput = ServiceLocator.GetService<IGameInput>();
            gameInput.DisablePlayerMovement();
        }
        else
        {
            _pauseGame = false;
            ClosePauseMenu();
            var gameInput = ServiceLocator.GetService<IGameInput>();
            gameInput.EnablePlayerMovement();
        }
    }
    public void OpenPauseMenu()
    {
        var guiManager = ServiceLocator.GetService<IGUIManager>();
        guiManager.OpenPlayerWindow("Windows/Player_Windows_prefs/PauseMenuDisplay");
        Time.timeScale = 0f;// Новый метод по пути
        Debug.Log("Open PauseMenu Window");

    }
    public void ClosePauseMenu()
    {
        var guiManager = ServiceLocator.GetService<IGUIManager>();
        guiManager.CloseCurrentWindow();
        Time.timeScale = 1.0f;
        Debug.Log("Close PauseMenu Window");

    }
}
