using AI.FSM.States;
using UnityEngine;

namespace AI.FSM
{
    public enum State
    {
        HEADHUNTER,
        CONQUORER,
        DEFENDER,
        CHASE,
        SHOOT,
    }

    public static class StateUtils
    {
        public static AbstractState GetNewState(State nextState)
        {
            switch (nextState)
            {
                case State.HEADHUNTER:
                    return new HeadhunterState();
                case State.CONQUORER:
                    return new ConquerState();
                case State.DEFENDER:
                    return new DefendState();
                default:
                    Debug.LogError("StateUtils: unsupported state");
                    return null;
            }
        }
    }
}