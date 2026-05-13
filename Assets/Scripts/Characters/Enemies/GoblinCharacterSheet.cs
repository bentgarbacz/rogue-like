using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinCharacterSheet : EnemyCharacterSheet
{

    public override void Awake()
    {
        
        base.Awake();
        stats.maxHealth = 10;
        stats.accuracy = 66;
        stats.minDamage = 1;
        stats.maxDamage = 4;
        level = 4;
        stats.speed = 8;
        stats.evasion = 50;

        dropTable = DropTableType.Goblin;
        title = "Goblin";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
    }

    public override void AggroBehavior()
    {

        base.AggroBehavior();
    }
}