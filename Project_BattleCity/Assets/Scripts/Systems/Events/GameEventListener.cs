using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour, IGameEventListener {
    [SerializeField]
    private GameEvent @event;

    [SerializeField]
    private UnityEvent response;

    public void OnEnable() {
        if (@event != null) @event.RegisterListener(this);
    }

    public void OnDisable() {
       // @event.UnregisterListener(this);
    }

    public void OnEventRaised() {
        response?.Invoke();
    }
}