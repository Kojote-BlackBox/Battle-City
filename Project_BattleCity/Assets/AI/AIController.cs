using System.Collections.Generic;
using AI.Fsm;
using AI.Fsm.States;
using UnityEngine;
using Utilities;
namespace AI
{
    public enum StarterState
    {
        HEADHUNTER,
        CONQUORER,
        DEFENDER,
    }

    public class AIController : MonoBehaviour
    {
        private static List<StarterState> _starterState = new List<StarterState>
        {
            StarterState.HEADHUNTER,
        };

        #region fsm
        private StateMachine _stateMachine;
        #endregion

        private void Start()
        {
            switch (_starterState[Utils.GetNumberInRange(0, _starterState.Count)])
            {
                case StarterState.HEADHUNTER:
                    _stateMachine = new StateMachine(gameObject, new HeadhunterState());
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }
    }
}
