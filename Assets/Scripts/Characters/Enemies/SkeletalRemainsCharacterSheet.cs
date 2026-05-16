using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalRemainsCharacterSheet : EnemyCharacterSheet
{

    private int dormancyTurns = 5;
    private bool willRespawn = false;
    private NPCGenerator npcGen;
    private MiniMapManager miniMapMgr;

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
        miniMapMgr = managers.GetComponent<UIActiveManager>().mapPanel.GetComponent<MiniMapManager>();
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
            
            GameObject skeleton = npcGen.CreateEnemy(NPCType.Skeleton, loc.Coord3d());
            skeleton.transform.rotation = transform.rotation;
            miniMapMgr.AddIcon(skeleton);
            return;
        }

        base.OnDeath();
    }
}