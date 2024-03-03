using AI.Fsm;
using AI.Fsm.States;
using UnityEngine;

namespace AI
{
    public class AiComponent : MonoBehaviour
    {
        private StateMachine _stateMachine;
        
        private void Awake()
        {
            _stateMachine = new StateMachine(gameObject,  new SearchForState());
        }

        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }
    }
}
