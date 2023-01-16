using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class StartNewGame : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;
        private IPersistentProgressService _persistentProgressService;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService, IPersistentProgressService persistentProgressService,
            IGameStateMachine gameStateMachine)
        {
            _saveLoadService = saveLoadService;
            _persistentProgressService = persistentProgressService;
            _gameStateMachine = gameStateMachine;
        }

        public void NewGame()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            _saveLoadService.CleanUp();
            _persistentProgressService.Progress = new PlayerProgress(Constants.InitialLevel);
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}