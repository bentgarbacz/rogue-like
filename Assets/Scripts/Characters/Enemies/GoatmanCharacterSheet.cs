using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatmanCharacterSheet : EnemyCharacterSheet
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 10;
        health = maxHealth;
        accuracy = 66;
        minDamage = 1;
        maxDamage = 4;
        level = 4;
        speed = 8;
        evasion = 50;

        dropTable = "Goatman";
        title = "Goatman";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
    }



    public override void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime)
    {

        base.AggroBehavior(playerCharacter, dum, cbm, waitTime);
    }
}
