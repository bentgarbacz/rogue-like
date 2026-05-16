using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicCharacterSheet : EnemyCharacterSheet
{

    private bool summoningSick = true;

    public override void Awake()
    {
        base.Awake();
        stats.maxHealth = 15;
        stats.accuracy = 75;
        stats.minDamage = 2;
        stats.maxDamage = 6;
        level = 5;
        stats.speed = 12;
        stats.evasion = 40;

        dropTable = DropTableType.Goblin; // TODO: Create DropTableType.Mimic when loot table is ready
        title = "Mimic";

        attackClip = Resources.Load<AudioClip>("Sounds/Mimic");
        audioSource.PlayOneShot(attackClip);
    }

    public override void AggroBehavior()
    {   

        if(summoningSick)
        {

            summoningSick = false;
            return;
        }

        base.AggroBehavior();
    }
}
