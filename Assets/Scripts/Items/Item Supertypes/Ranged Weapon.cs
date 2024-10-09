using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedWeapon : Equipment
{

    public ProjectileType projectile = ProjectileType.None;

    public RangedWeapon() : base()
    {

        bonusStatDictionary.Add(StatType.Range, 0);
    }
}
