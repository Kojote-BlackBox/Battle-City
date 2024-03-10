using UnityEngine;

namespace AI.FSM.States
{
    public class ChaseState : AbstractState
    {
        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            base.Enter(stateMachine, owner);

            _state = State.CHASE;
        }

        public override void Update(double deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}