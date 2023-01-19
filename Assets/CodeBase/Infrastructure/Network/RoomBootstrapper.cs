using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure.Network
{
    public class RoomBootstrapper : MonoBehaviourPunCallbacks
    {
        private GameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public override void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
           _gameStateMachine.Enter<LoadLevelState,string>(scene.name);
        }
    }
}