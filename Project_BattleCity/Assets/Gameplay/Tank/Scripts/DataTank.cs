using UnityEngine;
using Gameplay.Projectile;
using Gameplay.Health;

namespace Gameplay.Tank
{
    [CreateAssetMenu(fileName = "NewDataTank", menuName = "Data/Tank/Tank")]
    public class DataTank : ScriptableObject
    {
        [Header("Tank Parts")]
        public DataTankBody dataTankBody;
        public DataTankTurret dataTankTurret;
        public DataShell dataShell;
    }
}

