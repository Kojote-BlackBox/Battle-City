using Core.Pattern;
using UnityEngine;
using Core.Reference;

namespace Core.Event
{
    public class GameEventManager : Singleton<GameEventManager>
    {
        public GameEvent updateHealth;
        public GameEvent updateLive;
        public GameEvent spawnPlayer;
    }
}
