using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.WeaponsInventory;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activesState;

        public void Construct(SceneLoader sceneLoader,DiContainer container,LoadingCurtain curtain)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(
                    container.Resolve<GameStateMachine>(),
                    sceneLoader,
                    curtain,
                    container.Resolve<IGameFactory>(),
                    container.Resolve<IStaticDataService>(),
                    container.Resolve<ISaveLoadService>(),
                    container.Resolve<IPlayerWeaponsInventory>(),
                    container
                    ),
                [typeof(LoadProgressState)] = new LoadProgressState(
                    container.Resolve<GameStateMachine>(),
                    container.Resolve<IPersistentProgressService>(),
                    container.Resolve<ISaveLoadService>()
                    ),
                [typeof(GameLoopState)] = new GameLoopState(),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }


        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayLoadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activesState?.Exit();
            TState state = GetState<TState>();
            _activesState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}