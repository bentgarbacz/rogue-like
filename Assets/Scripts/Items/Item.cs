using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    
    public Sprite sprite;
    public string title;
    public int dropChance = 0;

    public void SetDropChance(int dc)
    {

        dropChance = Math.Clamp(dc, 0, 100);
    }
}
