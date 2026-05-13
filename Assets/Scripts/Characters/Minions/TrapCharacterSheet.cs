using System.Collections.Generic;
using UnityEngine;

// Trap that doesn't block tiles and explodes when a character occupies its tile
public class TrapCharacterSheet : MinionCharacterSheet
{
    public int explodeRadius = 1;
    public int explosionMinDamage = 3;
    public int explosionMaxDamage = 5;
    public GameObject explosionPrefab;
    private bool arming = true;
    
    public override void Awake()
    {
        base.Awake();
        
        stats.maxHealth = 1;
        stats.minDamage = 0;
        stats.maxDamage = 0;
        stats.speed = 0;
        stats.evasion = 0;
        stats.accuracy = 5000;
        
        level = 0;
        dropTable = DropTableType.None;
        title = "Trap";
    }



    private bool CheckForCharactersOnTile()
    {

        Tile tile = tileMgr.GetTile(loc.coord);

        foreach (GameObject entity in tile.entitiesOnTile)
        {

            CharacterSheet targetSheet = entity.GetComponent<CharacterSheet>();
            if (targetSheet != null && targetSheet != this)
            {

                return true;
            }
        }

        return false;
    }

    public override void OnDeath()
    {
        
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Explosion explosionComponent = explosionObject.GetComponent<Explosion>();
        
        if (explosionComponent != null)
        {
            explosionComponent.InitExplosion(explodeRadius, explosionMinDamage, explosionMaxDamage, this.gameObject);
            explosionComponent.SetExplosion();
        }
    }

    public override void AggroBehavior()
    {

        if(arming)
        {

            arming = false;
            return;
        }

        // Check if any character is on the same tile as this trap
        if (CheckForCharactersOnTile())
        {
            
            characterHealth.TakeDamage(stats.maxHealth);
        }
    }
}
