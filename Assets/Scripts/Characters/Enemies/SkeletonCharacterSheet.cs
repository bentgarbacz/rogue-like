using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterSheet : EnemyCharacterSheet
{

    public Mesh skeletonMesh;
    public Mesh pileOfBonesMesh;
    private int dormancyCountdown = 5;
    private const int dormancyTime = 5;
    private MeshFilter meshFilter;
    private bool isPileOfBones = false;

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
        meshFilter = GetComponent<MeshFilter>();
    }
    
    public override void AggroBehavior()
    {
        if(isPileOfBones)
        {
            
            PileOfBonesBehavior();
        
        }
        else
        {

            base.AggroBehavior();
        }
    }

    public override void OnDeath()
    {

        if (isPileOfBones)
        {

            base.OnDeath();
        }
        else
        {

            BecomePileOfBones();
        }
    }

    public override void Die()
    {

        if(!isPileOfBones)
        {
            
            OnDeath();
            return;
        }

        base.Die();
    }

    private void PileOfBonesBehavior()
    {
        
        dormancyCountdown--;

        if (dormancyCountdown <= 0)
        {

            dormancyCountdown = dormancyTime;
            BecomeSkeleton();
            return;
        }

        notificationManager.CreateNotificationOrder(2f, "...", Color.gray);
    }

    private void BecomeSkeleton()
    {
        characterHealth.Heal(stats.maxHealth);
        isPileOfBones = false;
        meshFilter.mesh = skeletonMesh;
        dormancyCountdown = dormancyTime;
    }

    private void BecomePileOfBones()
    {
        characterHealth.Heal(stats.maxHealth);
        characterHealth.TakeDamage(stats.maxHealth / 2);
        isPileOfBones = true;
        meshFilter.mesh = pileOfBonesMesh;
        dormancyCountdown = dormancyTime;
    }

    //public override void OnDeath()
    //{
    //    
    //    GameObject newSkeletalRemains = npcGen.CreateEnemy(NPCType.SkeletalRemains, loc.Coord3d());
    //    newSkeletalRemains.transform.rotation = transform.rotation;
    //    miniMapMgr.AddIcon(newSkeletalRemains);
    //    entityMgr.aggroEnemies.Add(newSkeletalRemains);
    //}
}
