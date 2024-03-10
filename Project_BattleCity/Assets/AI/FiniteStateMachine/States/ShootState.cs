using Gameplay.Tank;
using UnityEngine;
using Utilities;

namespace AI.Fsm.States
{
    public class ShootState : AbstractState
    {
        private TankTurret _componentTankTurret;

        private GameObject _gameObjectTarget;

        public ShootState(GameObject target)
        {
            _gameObjectTarget = target;
        }

        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            base.Enter(stateMachine, owner);

            _componentTankTurret = owner.GetComponentInChildren<TankTurret>();

            Debug.Log("AI: Entering shoot state");
        }

        public override void Update(double deltaTime)
        {
            ChangeDirectionToTarget();

            _componentTankTurret.Shoot();

            if (_gameObjectTarget == null)
            {
                StateMachine.ChangeState(new DefendState()); // TODO:
            }
        }

        public void ChangeDirectionToTarget()
        {
            if (_gameObjectTarget == null) return;

            var directionToTarget = Utils.CalculateDirectionToTarget(Owner.transform.position, _gameObjectTarget.transform.position);

            Debug.DrawRay(Owner.gameObject.transform.position, directionToTarget, Color.green);

            if (directionToTarget.magnitude > _componentTankTurret.GetRange())
            {
                StateMachine.ChangeState(new ChaseState()); // TODO:
            }

            var directionRotation = Utils.CalculateRotationToTarget(directionToTarget, _componentTankTurret.GetCurrentDirection());

            if (directionRotation > 0)
            {
                _componentTankTurret.RotateRight();
            }
            else if (directionRotation < 0)
            {
                _componentTankTurret.RotateLeft();
            }
        }

        public override void Exit()
        {
        }
    }
}