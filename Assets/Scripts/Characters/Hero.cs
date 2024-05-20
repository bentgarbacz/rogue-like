using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 20;
        health = maxHealth;
        accuracy = 80;
        minDamage = 5;
        maxDamage = 10;
        level = 1;
    }
}