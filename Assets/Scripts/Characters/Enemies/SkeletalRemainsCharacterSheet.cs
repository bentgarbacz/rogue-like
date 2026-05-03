using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalRemainsCharacterSheet : EnemyCharacterSheet
{

    private int dormancyTurns = 5;
    private bool willRespawn = false;
    private NPCGenerator npcGen;

    public override void Awake()
    {

        base.Awake();
        maxHealth = 5;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;
        evasion = 50;

        characterHealth.InitHealth(maxHealth);

        dropTable = DropTableType.Skeleton;
        title = "Skeletal Remains";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");

        npcGen = GameObject.Find("Map Generator").GetComponent<NPCGenerator>();
    }

    public override void AggroBehavior()
    {

        dormancyTurns--;

        if (dormancyTurns <= 0)
        {

            willRespawn = true;
            characterHealth.TakeDamage(maxHealth);
            return;
        }

        notificationManager.CreateNotificationOrder(2f, "...", Color.gray);
    }

    public override void OnDeath()
    {

        if(willRespawn)
        {
            
            npcGen.CreateEnemy(NPCType.Skeleton, loc.Coord3d());
            return;
        }

        base.OnDeath();
    }
}