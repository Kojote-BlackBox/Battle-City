using System.Net.NetworkInformation;
using UnityEngine;

namespace Core
{
    public static class GameConstants
    {
        #region tags
        public const string TagSpawn = "Spawn";
        public const string TagTank = "tank";
        public const string TagBunker = "bunker";
        public const string TagProjectile = "projectile";
        public const string TagPickup = "pickup";
        public const string TagEnemy = "enemy";
        public const string TagFriendly = "friendly";
        public const string TagHealth = "health";
        public const string TagTurret = "turret";
        public const string TagPlayer = "player";
        #endregion

        #region colors
        public static Color colorFriendly = Color.green;
        public static Color colorEnemy = Color.red;
        #endregion

        #region spatial
        public static int MapRectangleCount = 3;
        #endregion

        #region effects
        public static float GodShieldDuration = 2f;
        #endregion
    }
}