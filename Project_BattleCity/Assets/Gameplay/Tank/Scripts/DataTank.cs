using UnityEngine;
using Gameplay.Projectile;
using Gameplay.Health;

namespace Gameplay.Tank
{
    [CreateAssetMenu(fileName = "NewDataTank", menuName = "Data/Tank/Tank")]
    public class DataTank : ScriptableObject
    {
        public DataTankBody dataTankBody;
        public DataTankTurret dataTankTurret;
    }
}

