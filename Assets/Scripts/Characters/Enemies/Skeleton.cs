using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Character
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 8;
        health = maxHealth;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;
        evasion = 50;

        dropTable = "Skeleton";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");
    }
}
