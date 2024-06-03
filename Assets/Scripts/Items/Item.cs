using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    
    public Sprite sprite;
    public string title;
    public string description;
    public int dropChance;
    public string contextText;
    public AudioClip contextClip;

    public Item()
    {

        dropChance = 0;
        contextText = "N/A";
        contextClip = Resources.Load<AudioClip>("Sounds/Click");
    }

    public void SetDropChance(int dc)
    {

        dropChance = Math.Clamp(dc, 0, 100);
    }

    public virtual void Use()
    {

        return;
    }
}
