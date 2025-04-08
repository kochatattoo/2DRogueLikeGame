using UnityEngine;

    public class PlayState : IGameState
    {
        private GameStateManager _stateManager;

        public PlayState(GameStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public void Enter()
        {
            Debug.Log("Entering Play State");
            // Здесь инициализируем игровое состояние
			// Включаем игровые менеджеры
        	var gameInputManager = ServiceLocator.GetService<IGameInput>();
        	var mapManager = ServiceLocator.GetService<IMapManager>();
        	var guiManager = ServiceLocator.GetService<IGUIManager>();
        	var startScreenManager = ServiceLocator.GetService<IStartScreenManager>();

        	if (gameInputManager is MonoBehaviour gameInputScript) gameInputScript.gameObject.SetActive(true);
        	if (mapManager is MonoBehaviour mapScript) mapScript.gameObject.SetActive(true);
        	if (guiManager is MonoBehaviour guiScript) guiScript.gameObject.SetActive(true);
        	if (startScreenManager is MonoBehaviour startScreenScript) startScreenScript.gameObject.SetActive(true);

        	gameInputManager.StartManager();
        	mapManager.StartManager();
        	guiManager.StartManager();
        	startScreenManager.StartManager();

        	var audioManager = ServiceLocator.GetService<IAudioManager>();
        	audioManager.InitializePlayerAudio();
        }

        public void Update()
        {
            // Здесь обрабатываем игровую логику
        }

        public void Exit()
        {
            Debug.Log("Exiting Play State");
            // Здесь освобождаем ресурсы, если это необходимо
			var gameInputManager = ServiceLocator.GetService<IGameInput>();
			var mapManager = ServiceLocator.GetService<IMapManager>();
			var guiManager = ServiceLocator.GetService<IGUIManager>();
			var startScreenManager = ServiceLocator.GetService<IStartScreenManager>();

		   if (gameInputManager is MonoBehaviour gameInputScript) gameInputScript.gameObject.SetActive(false);
   	       if (mapManager is MonoBehaviour mapScript) mapScript.gameObject.SetActive(false);
           if (guiManager is MonoBehaviour guiScript) guiScript.gameObject.SetActive(false);
           if (startScreenManager is MonoBehaviour startScreenScript) startScreenScript.gameObject.SetActive(false);
        }
    }

