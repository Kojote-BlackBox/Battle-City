using Gameplay.Tank;
using UnityEngine;

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
                StateMachine.ChangeState(new SearchForState());
            }
        }

        public override void Exit()
        {
        }

        public void ChangeDirectionToTarget()
        {
            if (_gameObjectTarget == null) return;

            var vectorToTarget = _gameObjectTarget.transform.position - Owner.transform.position;

            Debug.DrawRay(Owner.gameObject.transform.position, vectorToTarget, Color.green);

            if (vectorToTarget.magnitude > _componentTankTurret.GetRange())
            {
                Debug.Log("AI: lost target");

                StateMachine.ChangeState(new SearchForState()); // TODO: follow target or search new one (probability)
            }

            var crossProduct = Vector3.Cross(vectorToTarget.normalized, _componentTankTurret.GetCurrentDirection());

            if (crossProduct == Vector3.zero)
            {
                return; // target is straight ahead
            }
            else if (crossProduct.z > 0)
            {
                Debug.Log("AI: rotating right");
                _componentTankTurret.RotateRight(); // target is on the right
            }
            else
            {
                Debug.Log("AI: rotating left");
                _componentTankTurret.RotateLeft(); // target is on the left
            }
        }
    }
}