using UnityEngine;
using UnityEngine.Events;

/*
This class is used for Events that have no arguments (Example: Exit game event)
*/

[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class SOVoidEventChannel : ScriptableObject {
    public UnityAction OnEventRaised;

    public void RaiseEvent() {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}

