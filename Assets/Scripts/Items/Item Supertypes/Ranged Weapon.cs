using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Equipment
{

    public string projectile = "";

    public RangedWeapon() : base()
    {

        bonusStatDictionary.Add("Range", 0);
    }
}
