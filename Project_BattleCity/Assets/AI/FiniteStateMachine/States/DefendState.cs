using System.Collections.Generic;
using Core;
using Core.Tag;
using Gameplay.Tank;
using UnityEngine;
using Utilities;

namespace AI.Fsm.States
{
    public class DefendState : AbstractState
    {
        private System.Random _random;

        private TankBody _tankBody;
        private TankTurret _tankTurret;

        private double _timeElapsedTankBodyChange;
        private double _timeElapsedLineOfSightCheck;
        private double _timeElapsedTankTurretChange;

        private double _directionChangeIntervalBody;
        private double _directionChangeIntervalTurret;

        private double _lineOfSightCheckInterval;

        private Vector3 _currentDirectionBody;
        private float _currentDirectionTurret;

        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            base.Enter(stateMachine, owner);

            Debug.Log("Entering Defend State");

            _tankBody = owner.GetComponentInChildren<TankBody>();
            _tankTurret = owner.GetComponentInChildren<TankTurret>();

            _random = new System.Random();

            _directionChangeIntervalBody = _random.NextDouble() * 2.0f + 1.0f;
            _directionChangeIntervalTurret = _random.NextDouble() * 2.0f + 0.5f;
            _lineOfSightCheckInterval = _random.NextDouble() * 0.5f + 0.10f;

            _timeElapsedTankBodyChange = _directionChangeIntervalBody;
            _timeElapsedTankTurretChange = _directionChangeIntervalTurret;
            _timeElapsedLineOfSightCheck = 0.0f;
        }

        public override void Update(double deltaTime)
        {
            _timeElapsedTankBodyChange += deltaTime;
            _timeElapsedLineOfSightCheck += deltaTime;

            if (_timeElapsedTankBodyChange >= _directionChangeIntervalBody)
            {
                _timeElapsedTankBodyChange = 0.0f;
                _currentDirectionBody = Utils.CalculcateRandomDirection2D();
            }

            if (_timeElapsedTankTurretChange >= _directionChangeIntervalTurret)
            {
                _timeElapsedTankTurretChange = 0.0f;
                _currentDirectionTurret = Utils.CalculcateRandomDirection1D();
            }

            if (_timeElapsedLineOfSightCheck >= _lineOfSightCheckInterval)
            {
                _timeElapsedLineOfSightCheck = 0.0f;

                IsTargetInLineOfSight();
            }

            _tankBody.Move(_currentDirectionBody.y);
            _tankBody.Rotate(_currentDirectionBody.x, 0f);
            _tankTurret.Rotate(_currentDirectionTurret, 0f);
        }

        public override void Exit()
        {
        }

        private bool IsTargetInLineOfSight()
        {
            Debug.Log("AI: check for target in line of sight");

            var position = Owner.transform.position;
            var hit = Physics2D.Raycast(position, _tankTurret.GetCurrentDirection(), _tankTurret.GetRange()); // TODO: use cone/field of view

            Debug.DrawRay(position, _tankTurret.GetCurrentDirection() * _tankTurret.GetRange(), Color.magenta, 1.0f);

            if (hit.collider == null || hit.collider.gameObject == Owner || hit.collider.gameObject == null)
            {
                return false;
            }

            var componentTagsTarget = hit.collider.gameObject.GetComponentInChildren<ComponentTags>();
            if (componentTagsTarget != null && componentTagsTarget.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagHealth)))
            {
                if (componentTagsTarget.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTank)))
                {
                    Debug.Log("AI: found a tank");
                }
                else if (componentTagsTarget.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagProjectile)))
                {
                    Debug.Log("AI: found a projectile");
                }
                else if (componentTagsTarget.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTurret)))
                {
                    Debug.Log("AI: found a turret");
                }

                if (!componentTagsTarget.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagEnemy)))
                {
                    StateMachine.ChangeState(new ShootState(hit.collider.gameObject));
                }
            }

            return false;
        }
    }
}