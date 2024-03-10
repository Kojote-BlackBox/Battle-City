using UnityEngine;

namespace AI.FSM
{
    public abstract class AbstractState
    {
        protected StateMachine _stateMachine;
        protected GameObject _owner;

        protected State _state;

        public virtual void Enter(StateMachine stateMachine, GameObject owner)
        {
            _stateMachine = stateMachine;
            _owner = owner;
        }

        public abstract void Update(double deltaTime);

        public abstract void Exit();
    }
}