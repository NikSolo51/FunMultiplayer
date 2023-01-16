using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public void Construct(ICoroutineRunner coroutineRunner,LoadingCurtain curtain,DiContainer diContainer)
        {
            SceneLoader sceneLoader =new SceneLoader(coroutineRunner);
            StateMachine.Construct(sceneLoader,diContainer,curtain);
        }
    }
}