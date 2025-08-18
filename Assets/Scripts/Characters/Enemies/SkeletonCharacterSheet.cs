using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterSheet : EnemyCharacterSheet
{

    public override void Awake()
    {
        
        base.Awake();
        maxHealth = 8;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;
        evasion = 50;

        characterHealth.InitHealth(maxHealth);

        dropTable = "Skeleton";
        title = "Skeleton";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");
    }
    
    public override void AggroBehavior(float waitTime)
    {

        base.AggroBehavior(waitTime);
    }
}
