using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    
    public Sprite sprite;
    public string title;
    public string description;
    public int dropChance = 0;
    public string contextText = "";

    public void SetDropChance(int dc)
    {

        dropChance = Math.Clamp(dc, 0, 100);
    }

    public virtual void Use()
    {

        return;
    }
}
