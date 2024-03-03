using UnityEngine;

namespace AI.Fsm.States
{
    public class ChaseState : AbstractState
    {
        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            base.Enter(stateMachine, owner);
        }

        public override void Update(double deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}