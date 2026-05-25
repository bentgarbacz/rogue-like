using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderCharacterSheet : EnemyCharacterSheet
{

    public float poisonChance;
    public int poisonDuration;
    public int poisonDamage;

    public override void Awake()
    {
        
        base.Awake();
        stats.maxHealth = 15;
        stats.accuracy = 100;
        stats.minDamage = 1;
        stats.maxDamage = 3;
        level = 3;
        stats.speed = 15;
        stats.evasion = 75;
        poisonChance = 0.3f;
        poisonDuration = 3;
        poisonDamage = 1;

        dropTable = DropTableType.None;
        title = "Spider";

        attackClip = Resources.Load<AudioClip>("Sounds/Spider");
    }

    protected override bool AttackEntity(Vector2Int coord)
    {

        GameObject targetEntity = null;

        foreach (GameObject entity in tileMgr.GetTile(coord).entitiesOnTile)
        {

            if (entity != null && entity.GetComponent<CharacterSheet>() != null)
            {

                targetEntity = entity;
            }
        }

        if (targetEntity == null || !combatSeq.CheckMeleeAttackValidity(this.gameObject, targetEntity))
        {

            return false;
        }

        Attack attack = new(this.gameObject, targetEntity, stats.minDamage, stats.maxDamage, stats.speed);
        Poison poison = new(targetEntity.GetComponent<CharacterSheet>(), 3, stats.minDamage);

        attack.AttachModifier(poison, poisonChance);
        combatSeq.AddAttack(attack);

        return true;
    }
}
