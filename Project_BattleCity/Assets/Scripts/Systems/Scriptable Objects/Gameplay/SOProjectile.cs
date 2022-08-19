using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Gameplay/Projectile")]
public class SOProjectile : ScriptableObject {

    public int penetration;
    public int damage;
    public int explosiveRadius;
    public int distance;
}
