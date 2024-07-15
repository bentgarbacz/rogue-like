using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Character
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 15;
        health = maxHealth;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 15;
        evasion = 75;

        dropTable = "Rat";

        attackClip = Resources.Load<AudioClip>("Sounds/Rat");
    }
}
