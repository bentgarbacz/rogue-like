using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderCharacterSheet : EnemyCharacterSheet
{

    public int poisonChance;
    public int poisonDuration;
    public int poisonDamage;

    public override void Awake()
    {
        
        base.Awake();
        maxHealth = 15;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 15;
        evasion = 75;
        poisonChance = 50;
        poisonDuration = 3;
        poisonDamage = 1;

        characterHealth.InitHealth(maxHealth);

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

        Attack attack = new(this.gameObject, targetEntity, minDamage, maxDamage, speed);
        Poison poison = new(targetEntity.GetComponent<CharacterSheet>(), 3, minDamage);

        attack.AttachStatusEffect(poison, 0.3f);
        combatSeq.AddAttack(attack);

        return true;
    }
}
