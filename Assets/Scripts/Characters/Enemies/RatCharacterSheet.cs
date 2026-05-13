using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RatCharacterSheet : EnemyCharacterSheet
{

    public override void Awake()
    {
        
        base.Awake();
        stats.maxHealth = 15;
        stats.accuracy = 100;
        stats.minDamage = 1;
        stats.maxDamage = 3;
        level = 3;
        stats.speed = 15;
        stats.evasion = 75;

        dropTable = DropTableType.Rat;
        title = "Rat";

        attackClip = Resources.Load<AudioClip>("Sounds/Rat");
    }

    public override void AggroBehavior()
    {

        base.AggroBehavior();
    }
}
