﻿using System.Collections.Generic;
using UnityEngine;

namespace Core.Event
{
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Events/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

        public string additionalInfo;

        public void Raise()
        {
            for (var i = _listeners.Count - 1; i >= 0; --i)
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}
