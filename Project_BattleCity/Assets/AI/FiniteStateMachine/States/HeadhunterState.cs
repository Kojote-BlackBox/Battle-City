using UnityEngine;
using Gameplay.Tank;
using Core.Track;
using Utilities;

namespace AI.FSM.States
{
    public class HeadhunterState : AbstractState
    {
        private GameObject _target = null;

        private TankBody _tankBody;
        private TankTurret _tankTurret;

        private Vector2 _keepDistance;

        private float _currentDirectionTurret;

        private double _directionChangeIntervalTurret;
        private double _timeElapsedTankTurretChange;

        public HeadhunterState()
        {
            _state = State.HEADHUNTER;
        }

        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            Debug.Log("Entering Headhunter State");

            base.Enter(stateMachine, owner);

            _tankBody = owner.GetComponentInChildren<TankBody>();
            _tankTurret = owner.GetComponentInChildren<TankTurret>();

            _keepDistance = new Vector2(0.5f, _tankTurret.GetRange());

            _directionChangeIntervalTurret = Utils.CalculateRandomWithMinMax(0.5f, 2.0f);
            _timeElapsedTankTurretChange = _directionChangeIntervalTurret;
        }

        public override void Update(double deltaTime)
        {
            _timeElapsedTankTurretChange += deltaTime;

            if (_target == null && TrackManager.Instance.player.gameObject != null)
            {
                _target = TrackManager.Instance.player.gameObject;
            }

            if (_target == null) return;

            _keepDistance.y = _tankTurret.GetRange();

            // TODO: interval for direction change

            var directionToTarget = Utils.CalculateDirectionToTarget(_owner.transform.position, _target.transform.position);

            Debug.DrawRay(_owner.gameObject.transform.position, directionToTarget, Color.red);

            var directionRotationToTarget = Utils.CalculateRotationToTarget(directionToTarget, _tankBody.GetCurrentDirection());

            _tankBody.Rotate(directionRotationToTarget, 0f);

            var distanceToTarget = Vector2.Distance(_owner.transform.position, _target.transform.position);

            if (distanceToTarget >= _keepDistance.x && distanceToTarget <= _keepDistance.y)
            {
                _stateMachine.ChangeState(new ShootState(_target, _state));

                _tankBody.Move(0f);
            }
            else
            {
                _tankBody.Move(1f);
            }

            _tankTurret.Rotate(directionRotationToTarget, 0f);

            /*if (distanceToTarget <= _keepDistance.y * 3)
            {
                _tankTurret.Rotate(directionRotationToTarget, 0f);
            }
            else
            {
                if (_timeElapsedTankTurretChange >= _directionChangeIntervalTurret)
                {
                    _timeElapsedTankTurretChange = 0.0f;
                    _currentDirectionTurret = Utils.CalculcateRandomDirection1D();
                }

                _tankTurret.Rotate(_currentDirectionTurret, 0f);
            }*/
        }

        public override void Exit()
        {
        }
    }
}