using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatmanCharacterSheet : EnemyCharacterSheet
{

    public override void Awake()
    {
        
        base.Awake();
        maxHealth = 10;
        accuracy = 66;
        minDamage = 1;
        maxDamage = 4;
        level = 4;
        speed = 8;
        evasion = 50;

        characterHealth.InitHealth(maxHealth);

        dropTable = "Goatman";
        title = "Goatman";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
    }



    public override void AggroBehavior()
    {

        base.AggroBehavior();
    }
}
