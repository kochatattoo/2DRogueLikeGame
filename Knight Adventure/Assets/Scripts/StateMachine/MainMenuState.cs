using UnityEngine;

    public class MainMenuState: IGameState
    {
        private GameStateManager _stateManager;

        public MainMenuState(GameStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public void Enter()
        {
            Debug.Log("Entering Main Menu State");
            // Здесь можно добавить код для инициализации состояния меню
			var menuManager = FindObjectOfType<MainMenuManager>();
			if (menuManager != null)
			{
				menuManager.StartManager();
			}
			
        }

        public void Update()
        {
            // Здесь можно обработать ввод пользователя для изменения состояния
        }

        public void Exit()
        {
            Debug.Log("Exiting Main Menu State");
            // Здесь можно добавить код для очистки состояния
        }
    }

