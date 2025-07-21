using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RatCharacterSheet : EnemyCharacterSheet
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 15;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 15;
        evasion = 75;

        characterHealth.InitHealth(maxHealth);

        dropTable = "Rat";
        title = "Rat";

        attackClip = Resources.Load<AudioClip>("Sounds/Rat");
    }

    public override void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime)
    {

        base.AggroBehavior(playerCharacter, dum, cbm, waitTime);
    }
}
