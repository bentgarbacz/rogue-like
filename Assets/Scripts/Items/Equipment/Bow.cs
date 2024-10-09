using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : RangedWeapon
{

    public Bow(int dropChance = 0 )
    {

        this.title = "Bow";
        this.description = "Ranged weapon, fires arrows.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Bow");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.MainHand;

        this.bonusStatDictionary[StatType.MinDamage] = 1;
        this.bonusStatDictionary[StatType.MaxDamage] = 3;
        this.bonusStatDictionary[StatType.Accuracy] = 20;
        this.bonusStatDictionary[StatType.Range] = 8;
        this.bonusStatDictionary[StatType.DexterityRequirement] = 10;

        this.projectile = ProjectileType.Arrow;

        SetDropChance(dropChance);
    }
}
