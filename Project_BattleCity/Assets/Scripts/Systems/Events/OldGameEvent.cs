//https://blog.devgenius.io/scriptableobject-game-events-1f3401bbde72

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Events/Game Event")]
public class GameEvent : ScriptableObject
{
    private readonly List<IGameEventListener> eventListener = new List<IGameEventListener>();

    public void Raise() {
        for (int i = eventListener.Count - 1; i >= 0; i--)
            eventListener[i].OnEventRaised();
    }

    public void RegisterListener(IGameEventListener listener)
    {
        if(eventListener.Contains(listener))
            eventListener.Add(listener);
    }

    public void UnregisterListener(IGameEventListener listener) {
        if(eventListener.Contains(listener))
            eventListener.Remove(listener);
    }
}
