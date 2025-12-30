using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderCharacterSheet : EnemyCharacterSheet
{

    public override void Awake()
    {
        
        base.Awake();
        maxHealth = 15;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 15;
        evasion = 75;

        characterHealth.InitHealth(maxHealth);

        dropTable = "Spider";
        title = "Spider";

        attackClip = Resources.Load<AudioClip>("Sounds/Spider");
    }

    public override void AggroBehavior()
    {

        base.AggroBehavior();
    }
}
