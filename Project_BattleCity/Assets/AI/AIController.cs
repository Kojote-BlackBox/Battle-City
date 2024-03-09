using AI.Fsm;
using AI.Fsm.States;
using UnityEngine;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        #region fsm
        private StateMachine _stateMachine;
        #endregion

        private void Awake()
        {
            _stateMachine = new StateMachine(gameObject, new SearchForState());
        }

        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }
    }
}
