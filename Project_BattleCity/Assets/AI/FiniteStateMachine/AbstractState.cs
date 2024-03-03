using UnityEngine;

namespace AI.Fsm
{
    public abstract class AbstractState
    {
        protected StateMachine StateMachine;
        protected GameObject Owner;
        
        public virtual void Enter(StateMachine stateMachine, GameObject owner)
        {
            StateMachine = stateMachine;
            Owner = owner;
        }

        public abstract void Update(double deltaTime);

        public abstract void Exit();
    }
}