using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterSheet : EnemyCharacterSheet
{
    private NPCGenerator npcGen;

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
