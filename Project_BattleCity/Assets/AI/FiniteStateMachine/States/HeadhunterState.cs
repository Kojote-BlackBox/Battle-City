using UnityEngine;
using Gameplay.Tank;
using Core.Track;
using Utilities;
using UnityEditorInternal;

namespace AI.Fsm.States
{
    public class HeadhunterState : AbstractState
    {
        private GameObject _target = null;

        private TankBody _tankBody;
        private TankTurret _tankTurret;

        private Vector2 _keepDistance;

        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            Debug.Log("Entering Headhunter State");

            base.Enter(stateMachine, owner);

            _tankBody = owner.GetComponentInChildren<TankBody>();
            _tankTurret = owner.GetComponentInChildren<TankTurret>();

            _keepDistance = new Vector2(0.5f, _tankTurret.GetRange());
        }

        public override void Update(double deltaTime)
        {
            if (_target == null && TrackManager.Instance.player.gameObject != null)
            {
                _target = TrackManager.Instance.player.gameObject;
            }

            if (_target == null) return;

            _keepDistance.y = _tankTurret.GetRange();

            // TODO: interval for direction change

            var directionToTarget = Utils.CalculateDirectionToTarget(Owner.transform.position, _target.transform.position);

            Debug.DrawRay(Owner.gameObject.transform.position, directionToTarget, Color.red);

            var directionRotationToTarget = Utils.CalculateRotationToTarget(directionToTarget, _tankBody.GetCurrentDirection());

            _tankBody.Rotate(directionRotationToTarget, 0f);

            var distanceToTarget = Vector2.Distance(Owner.transform.position, _target.transform.position);

            Debug.Log("Distance to target: " + distanceToTarget + " - Keep distance vector: " + _keepDistance);

            if (distanceToTarget >= _keepDistance.x && distanceToTarget <= _keepDistance.y)
            {
                StateMachine.ChangeState(new ShootState(_target));

                _tankBody.Move(0f);
            }
            else
            {
                _tankBody.Move(1f);
            }

            if (distanceToTarget <= _keepDistance.y * 2)
            {
                _tankTurret.Rotate(directionRotationToTarget, 0f);
            }
            else
            {
                _tankTurret.Rotate(Utils.CalculcateRandomDirection1D(), 0f);
            }
        }

        public override void Exit()
        {
        }
    }
}