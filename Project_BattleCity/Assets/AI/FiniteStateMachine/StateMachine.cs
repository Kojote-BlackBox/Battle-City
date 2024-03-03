using UnityEngine;

namespace AI.Fsm
{
    public class StateMachine
    {
        private AbstractState _currentState;
        private readonly GameObject _owner;

        public StateMachine(GameObject owner, AbstractState initialState)
        {
            _owner = owner;
            
            ChangeState(initialState);
        }

        public void Update(double deltaTime)
        {
            _currentState?.Update(deltaTime);
        }

        public void ChangeState(AbstractState nextState)
        {
            if (nextState == null) return;
            
            _currentState?.Exit();
            
            _currentState = nextState;
            
            _currentState.Enter(this, _owner);
        }
    }
}