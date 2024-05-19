using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Consumable
{

    public Potion(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/potion");
        this.title = "Potion";
        this.dropChance = dropChance;
    }


}
