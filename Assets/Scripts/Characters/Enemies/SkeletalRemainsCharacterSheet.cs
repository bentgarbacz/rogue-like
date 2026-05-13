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
        stats.maxHealth = 5;
        stats.accuracy = 100;
        stats.minDamage = 1;
        stats.maxDamage = 3;
        level = 3;
        stats.speed = 11;
        stats.evasion = 50;

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
            characterHealth.TakeDamage(stats.maxHealth);
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