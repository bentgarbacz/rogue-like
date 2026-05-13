using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrap : Spell
{
    private NPCGenerator npcGen;
    private TileManager tileMgr;

    public ThrowTrap()
    {
        this.spellType = SpellType.ThrowTrap;
        this.targeted = true;
        this.cooldown = 2;
        this.manaCost = 1;
        this.range = 5;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Trap");

        npcGen = GameObject.Find("Map Generator").GetComponent<NPCGenerator>();
        tileMgr = GameObject.Find("System Managers").GetComponent<TileManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {
        Tile targetTile = target.GetComponent<Tile>();

        // Check if target tile is valid and unoccupied
        if (!targetTile || tileMgr.occupiedlist.Contains(targetTile.loc.coord))
        {
            return false;
        }

        // Create the trap at the target tile
        GameObject trap = npcGen.CreateTrap(targetTile.transform.position);

        
        trap.transform.position = caster.transform.position;
        trap.GetComponent<MeshRenderer>().enabled = true;
        trap.GetComponent<MoveToTarget>().SetTarget(targetTile.transform.position);

        return trap != null;
    }
}
