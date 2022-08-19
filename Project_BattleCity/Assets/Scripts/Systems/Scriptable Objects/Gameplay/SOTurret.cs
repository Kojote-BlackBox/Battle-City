using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurret", menuName = "Gameplay/Tank/Turret")]
public class SOTurret : ScriptableObject {

    public float rotationSpeed;
    public float reloadTime;
    public float projectileSpeed;
}
