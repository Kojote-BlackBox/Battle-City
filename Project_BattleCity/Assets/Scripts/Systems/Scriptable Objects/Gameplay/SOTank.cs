using UnityEngine;

[CreateAssetMenu(fileName = "NewTank", menuName = "Gameplay/Tank/Tank")]
public class SOTank : ScriptableObject {

    public SOChassie chassie;
    public SOTurret turret;
    public SOProjectile projectile;
}
