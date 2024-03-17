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

        public GameEvent EventKeyEscapeOnPush;
        public GameEvent EventKeyWOnPush;
        public GameEvent EventKeySOnPush;
        public GameEvent EventKeyAOnPush;
        public GameEvent EventKeyDOnPush;
        public GameEvent EventKeySpaceOnPush;
        public GameEvent EventKeyQOnPush;
        public GameEvent EventKeyEOnPush;
        public GameEvent EventKeyPOnPush;
    }
}
