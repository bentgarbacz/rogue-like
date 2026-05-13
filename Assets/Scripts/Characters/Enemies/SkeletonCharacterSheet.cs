using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterSheet : EnemyCharacterSheet
{
    private NPCGenerator npcGen;

    public override void Awake()
    {
        base.Awake();
        stats.maxHealth = 8;
        stats.accuracy = 100;
        stats.minDamage = 1;
        stats.maxDamage = 3;
        level = 3;
        stats.speed = 11;
        stats.evasion = 50;

        dropTable = DropTableType.Skeleton;
        title = "Skeleton";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");
        npcGen = GameObject.Find("Map Generator").GetComponent<NPCGenerator>();
    }
    
    public override void AggroBehavior()
    {
        base.AggroBehavior();
    }

    public override void OnDeath()
    {
        
        GameObject newSkeletalRemains = npcGen.CreateEnemy(NPCType.SkeletalRemains, loc.Coord3d());
        entityMgr.aggroEnemies.Add(newSkeletalRemains);
    }
}
