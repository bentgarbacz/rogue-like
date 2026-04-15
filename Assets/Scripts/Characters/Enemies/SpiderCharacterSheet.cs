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

    private GameObject GetTargetAtCoord(Vector2Int coord)
    {
        foreach (GameObject entity in tileMgr.GetTile(coord).entitiesOnTile)
        {
            if (entity != null && entity.GetComponent<CharacterSheet>() != null)
            {
                return entity;
            }
        }
        return null;
    }

    public override void AggroBehavior()
    {

        if (!GetAggroStatus())
        {

            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }
        
        List<Dictionary<Vector2Int, float>> mapsOfInterest = new(){djm.npcMap, djm.playerMap};
        Vector2Int targetCoord = GetNeighborTileOfMostInterest(loc.coord, mapsOfInterest);
        float tileInterestVal = Mathf.Min(djm.GetPlayerMapValue(targetCoord), djm.GetNpcMapValue(targetCoord));
        
        if(tileInterestVal == 0)
        {
            GameObject target = GetTargetAtCoord(targetCoord);
            if(AttackEntity(targetCoord) && target != null)
            {
                if(Random.Range(0, 100) < poisonChance)
                {
                    target.GetComponent<StatusEffectManager>().AddEffect(new Poison(target.GetComponent<CharacterSheet>(), poisonDuration, poisonDamage, entityMgr));
                    target.GetComponent<TextNotificationManager>().CreateNotificationOrder(target.transform.position, 3f, "Poisoned", Color.green, 1f);
                }
            }
        }else
        {

            movementManager.AddMovement(this, targetCoord);
        }

    }
}
