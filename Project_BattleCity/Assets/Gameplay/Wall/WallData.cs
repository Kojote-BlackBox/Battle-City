using Gameplay.Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Wall
{
    [CreateAssetMenu(fileName = "NewDataWall", menuName = "Data/Wall")]
    public class WallData : DataHealth
    {
        [FormerlySerializedAs("DefensivePower")] public int defensivePower;
        public Sprite destroyedWall;
    }
}
