using System;
using UnityEngine;

namespace CodeBase.PlayerScripts
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField]private Animator _animator;

        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _walkingStateHash = Animator.StringToHash("Walking");

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }

        public void Move(float velocityMagnitude)
        {
            _animator.SetBool("IsMove",true);
            _animator.SetFloat("Speed", velocityMagnitude);
        }

        public void StopMoving(float velocityMagnitude)
        {
            _animator.SetBool("IsMove",false);

            _animator.SetFloat("Speed", velocityMagnitude);
        }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
            {
                state = AnimatorState.Idle;
            }
            else if (stateHash == _walkingStateHash)
            {
                state = AnimatorState.Walking;
            }
            else
            {
                state = AnimatorState.Unknown;
            }

            return state;
        }
    }
}