using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkull : Spell
{

    private NPCGenerator npcGen;
    private TileManager tileManager;
    private DungeonManager dum;
    private NPCType npcType = NPCType.Skull;

    public SummonSkull()
    {

        this.spellType = SpellType.SummonSkull;
        this.targeted = true;
        this.cooldown = 3;
        this.manaCost = 2;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/SkullMinion");

        npcGen = GameObject.Find("Map Generator").GetComponent<NPCGenerator>();

        GameObject managers = GameObject.Find("System Managers");

        tileManager = managers.GetComponent<TileManager>();
        dum = managers.GetComponent<DungeonManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        Tile targetTile = target.GetComponent<Tile>();
        CharacterSheet targetCharacter = target.GetComponent<CharacterSheet>();

        if (targetCharacter)
        {

            foreach (Vector2Int d in Direction2D.DirectionsList())
            {

                Vector2Int checkCoord = targetCharacter.loc.coord + d;

                if (dum.dungeonCoords.Contains(checkCoord) && !dum.occupiedlist.Contains(checkCoord))
                {

                    targetTile = tileManager.tileDict[checkCoord];
                }

            }
        }
        
        if (!targetTile || dum.occupiedlist.Contains(targetTile.loc.coord))
        {

            return false;
        }

        GameObject minion = npcGen.CreateNPC(npcType, targetTile.transform.position);

        if (!minion)
        {

            return false;
        }

        minion.GetComponent<ObjectVisibility>().SetVisibility(true);

        return true;
    }
}
