using Gameplay.Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Wall
{
    public class WallDataComponent : MonoBehaviour
    {
        [FormerlySerializedAs("WallData")] public WallData wallData;
    }
}
