using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState: IGameState
    {
        private GameStateManager _stateManager;
		private MainMenuManager _mainMenuManager;
        private GameInput _gameInput;

        public MainMenuState(GameStateManager stateManager, MainMenuManager mainMenuManager)
        {
           _stateManager = stateManager;
           _mainMenuManager = mainMenuManager;
        }

        public void Enter()
        {
            Debug.Log("Entering Main Menu State");
            // Здесь можно добавить код для инициализации состояния меню
			_mainMenuManager = _stateManager.FindObject<MainMenuManager>();
			if (_mainMenuManager != null)
			{
				_mainMenuManager.StartManager();
			}
			
        }

        public void Update()
        {
           // _stateManager.ChangeState(new PlayState(_stateManager)); 
        }

        public void Exit()
        {
            Debug.Log("Exiting Main Menu State");
            // Здесь можно добавить код для очистки состояния
			if(_mainMenuManager != null)
			{
				_mainMenuManager.DisableManager();
			}
        }
    }

