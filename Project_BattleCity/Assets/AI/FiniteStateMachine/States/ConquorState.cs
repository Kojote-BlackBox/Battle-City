using UnityEngine;

namespace AI.FSM.States
{
    public class ConquerState : AbstractState
    {
        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            base.Enter(stateMachine, owner);

            _state = State.CONQUORER;
        }

        public override void Update(double deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}