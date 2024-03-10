using System.Collections.Generic;
using AI.FSM;
using AI.FSM.States;
using UnityEngine;
using Utilities;
namespace AI
{
    public class AIController : MonoBehaviour
    {
        private static List<State> _starterState = new List<State>
        {
            State.HEADHUNTER,
        };

        #region fsm
        private StateMachine _stateMachine;
        #endregion

        private void Start()
        {
            _stateMachine = new StateMachine(gameObject, StateUtils.GetNewState(_starterState[Utils.GetNumberInRange(0, _starterState.Count)]));
        }

        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }
    }
}
