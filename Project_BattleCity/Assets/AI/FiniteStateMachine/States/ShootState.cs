using Gameplay.Tank;
using UnityEngine;
using Utilities;

namespace AI.FSM.States
{
    public class ShootState : AbstractState
    {
        private TankTurret _componentTankTurret;

        private GameObject _gameObjectTarget;

        private State _previousState;

        public ShootState(GameObject target, State previousState)
        {
            _gameObjectTarget = target;
            _previousState = previousState;
        }

        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            base.Enter(stateMachine, owner);

            _state = State.SHOOT;

            _componentTankTurret = owner.GetComponentInChildren<TankTurret>();

            Debug.Log("AI: Entering shoot state");
        }

        public override void Update(double deltaTime)
        {
            if (_gameObjectTarget == null) _stateMachine.ChangeState(StateUtils.GetNewState(_previousState));

            var directionToTarget = Utils.CalculateDirectionToTarget(_owner.transform.position, _gameObjectTarget.transform.position);
            var distanceToTarget = Vector2.Distance(_owner.transform.position, _gameObjectTarget.transform.position);

            ChangeDirectionToTarget(directionToTarget);

            if (distanceToTarget > _componentTankTurret.GetRange()) _stateMachine.ChangeState(StateUtils.GetNewState(_previousState));

            _componentTankTurret.Shoot();
        }

        public void ChangeDirectionToTarget(Vector2 directionToTarget)
        {
            Debug.DrawRay(_owner.gameObject.transform.position, directionToTarget, Color.green);

            _componentTankTurret.Rotate(Utils.CalculateRotationToTarget(directionToTarget, _componentTankTurret.GetCurrentDirection()), 0f);
        }

        public override void Exit()
        {
        }
    }
}