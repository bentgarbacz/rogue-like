using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Consumable
{

    public Potion()
    {
        this.sprite = Resources.Load<Sprite>("Pixel Art/potion");
        this.title = "Potion";
    }


}
