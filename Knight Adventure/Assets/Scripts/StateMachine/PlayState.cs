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
        }

        public void Update()
        {
            // Здесь обрабатываем игровую логику
        }

        public void Exit()
        {
            Debug.Log("Exiting Play State");
            // Здесь освобождаем ресурсы, если это необходимо
        }
    }

