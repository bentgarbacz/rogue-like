using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Character
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 15;
        health = maxHealth;
        accuracy = 80;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;

        dropTable = "Skeleton";
    }
}
