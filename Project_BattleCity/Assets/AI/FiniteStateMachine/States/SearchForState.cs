using System.Collections.Generic;
using Core;
using Core.Tag;
using Gameplay.Tank;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;

namespace AI.Fsm.States
{
    //TODO: A* Path finding sometimes include walls -> shoot them
    //TODO: Different behaviour based on tank (chase and shoot, search for base, search for player, shoot and run)
    //TODO: Tanks with Upgrades -> probability that the ai makes it hard for the player to catch it (flee if player is near)
    //TODO: Probability that the ai defends against bullets
    public class SearchForState : AbstractState
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

        private List<Vector3> _directionsTankBody;
        private List<float> _directionsTankTurret;

        private Vector3 _currentDirectionBody;
        private float _currentDirectionTurret;

        public override void Enter(StateMachine stateMachine, GameObject owner)
        {
            base.Enter(stateMachine, owner);

            Debug.Log("Entering SearchFor State");

            _tankBody = owner.GetComponentInChildren<TankBody>();
            _tankTurret = owner.GetComponentInChildren<TankTurret>();

            _random = new System.Random();

            _directionChangeIntervalBody = _random.NextDouble() * 2.0f + 1.0f;
            _directionChangeIntervalTurret = _random.NextDouble() * 2.0f + 0.5f;
            _lineOfSightCheckInterval = _random.NextDouble() * 0.5f + 0.10f;

            _timeElapsedTankBodyChange = _directionChangeIntervalBody;
            _timeElapsedTankTurretChange = _directionChangeIntervalTurret;
            _timeElapsedLineOfSightCheck = 0.0f;

            _directionsTankBody = new List<Vector3>
            {
                new Vector3(0.0f, 0.0f),
                new Vector3(0.0f, -1.0f),
                new Vector3(0.0f, 1.0f),
                new Vector3(-1.0f, 0.0f),
                new Vector3(-1.0f, 1.0f),
                new Vector3(-1.0f, -1.0f),
                new Vector3(1.0f, 0.0f),
                new Vector3(1.0f, 1.0f),
                new Vector3(1.0f, -1.0f),
            };

            _directionsTankTurret = new List<float> {
                0.0f,
                -1.0f,
                1.0f
            };
        }

        public override void Update(double deltaTime)
        {
            _timeElapsedTankBodyChange += deltaTime;
            _timeElapsedLineOfSightCheck += deltaTime;

            if (_timeElapsedTankBodyChange >= _directionChangeIntervalBody)
            {
                _timeElapsedTankBodyChange = 0.0f;
                _currentDirectionBody = GenerateRandomDirectionBody();
            }

            if (_timeElapsedTankTurretChange >= _directionChangeIntervalTurret)
            {
                _timeElapsedTankTurretChange = 0.0f;
                _currentDirectionTurret = GenerateRandomDirectionTurret();
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

                if (!componentTagsTarget.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagEnemy))) {
                    StateMachine.ChangeState(new ShootState(hit.collider.gameObject));
                }
            }

            return false;
        }

        private Vector3 GenerateRandomDirectionBody()
        {
            var randomDirectionIndex = _random.Next(0, _directionsTankBody.Count);
            return _directionsTankBody[randomDirectionIndex];
        }

        private float GenerateRandomDirectionTurret()
        {
            var randomDirectionIndex = _random.Next(0, _directionsTankTurret.Count);
            return _directionsTankTurret[randomDirectionIndex];
        }
    }
}