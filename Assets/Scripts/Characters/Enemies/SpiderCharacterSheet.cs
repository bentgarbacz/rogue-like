using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderCharacterSheet : EnemyCharacterSheet
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

        dropTable = "Spider";
        title = "Spider";

        attackClip = Resources.Load<AudioClip>("Sounds/Spider");
    }

    public override void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime)
    {

        base.AggroBehavior(playerCharacter, dum, cbm, waitTime);
    }
}
