using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain CurtainPrefab;
        private Game _game;
        private GameStateMachine _gameStateMachine;
        private DiContainer _diContainer;
        
        [Inject]
        public void Construct(GameStateMachine gameStateMachine,DiContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
            Initialize();
        }

        private void Initialize()
        {
            _game = new Game(_gameStateMachine);
            _game.Construct(this, Instantiate(CurtainPrefab),_diContainer);
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
        }
    }
}