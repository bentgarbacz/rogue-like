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
        accuracy = 66;
        minDamage = 1;
        maxDamage = 5;
        level = 4;

        dropTable = "Goblin";
    }
}