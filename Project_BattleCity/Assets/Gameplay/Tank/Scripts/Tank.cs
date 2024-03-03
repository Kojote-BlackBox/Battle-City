using Core.Aim;
using Gameplay.Projectile;
using UnityEngine;

namespace Gameplay.Tank
{
    public class Tank : MonoBehaviour
    {
        #region data
        [Header("Data")]
        [SerializeField] private DataTankBody _dataTankBody;
        [SerializeField] private DataTankTurret _dataTankTurret;
        [SerializeField] private DataShell _dataShell;
        #endregion

        #region components
        private TankBody _componentTankBody;
        private TankTurret _componentTankTurret;
        #endregion

        void Start()
        {
            _componentTankBody = gameObject.GetComponentInChildren<TankBody>();
            _componentTankTurret = gameObject.GetComponentInChildren<TankTurret>();

            _dataTankTurret.dataShell = _dataShell;
            _componentTankTurret.SetDataTankTurret(_dataTankTurret);
            _componentTankBody.SetDataTankBody(_dataTankBody);
        }

        public void ChangeTankBody(DataTankBody newTankBody)
        {
            _dataTankBody = newTankBody;
            _componentTankBody.SetDataTankBody(_dataTankBody);
        }

        public void ChangeTankTurret(DataTankTurret newTankTurret)
        {
            _dataTankTurret = newTankTurret;
            _dataTankTurret.dataShell = _dataShell;
            _componentTankTurret.SetDataTankTurret(_dataTankTurret);

            var componentAimIndicator = GetComponent<ComponentAimIndicator>();
            if (componentAimIndicator != null) componentAimIndicator.SetLength(_componentTankTurret.GetRange());
        }

        public void ChangeShell(DataShell newShell)
        {
            _dataShell = newShell;
            _dataTankTurret.dataShell = _dataShell;
            _componentTankTurret.SetDataTankTurret(_dataTankTurret);
        }
    }
}

