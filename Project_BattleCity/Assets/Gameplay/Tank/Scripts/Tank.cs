using Core.Aim;
using Gameplay.Projectile;
using UnityEngine;

namespace Gameplay.Tank
{
    public class Tank : MonoBehaviour
    {
        #region data
        [Header("Data")]
        [SerializeField] private DataTank _dataTankInitial;

        private DataTankBody _dataTankBodyCurrent;
        private DataTankTurret _dataTankTurretCurrent;
        private DataShell _dataShellCurrent;
        #endregion

        #region components
        private TankBody _componentTankBody;
        private TankTurret _componentTankTurret;
        #endregion

        void Start()
        {
            _componentTankBody = gameObject.GetComponentInChildren<TankBody>();
            _componentTankTurret = gameObject.GetComponentInChildren<TankTurret>();

            _dataTankBodyCurrent = _dataTankInitial.dataTankBody;
            _dataTankTurretCurrent = _dataTankInitial.dataTankTurret;
            _dataShellCurrent = _dataTankInitial.dataShell;

            _dataTankTurretCurrent.dataShell = _dataShellCurrent;
            _componentTankTurret.SetDataTankTurret(_dataTankTurretCurrent);
            _componentTankBody.SetDataTankBody(_dataTankBodyCurrent);
        }

        public void ChangeTankBody(DataTankBody newTankBody)
        {
            _dataTankBodyCurrent = newTankBody;
            _componentTankBody.SetDataTankBody(_dataTankBodyCurrent);
        }

        public void ChangeTankTurret(DataTankTurret newTankTurret)
        {
            _dataTankTurretCurrent = newTankTurret;
            _dataTankTurretCurrent.dataShell = _dataShellCurrent;
            _componentTankTurret.SetDataTankTurret(_dataTankTurretCurrent);

            var componentAimIndicator = GetComponent<ComponentAimIndicator>();
            if (componentAimIndicator != null) componentAimIndicator.SetLength(_componentTankTurret.GetRange());
        }

        public void ChangeShell(DataShell newShell)
        {
            _dataShellCurrent = newShell;
            _dataTankTurretCurrent.dataShell = _dataShellCurrent;
            _componentTankTurret.SetDataTankTurret(_dataTankTurretCurrent);
        }
    }
}

