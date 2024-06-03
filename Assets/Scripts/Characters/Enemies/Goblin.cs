using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Character
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 20;
        health = maxHealth;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 5;
        level = 4;
        speed = 8;
        evasion = 50;

        dropTable = "Goblin";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
    }
}