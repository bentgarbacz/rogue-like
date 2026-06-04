using System.Collections.Generic;
using UnityEngine;

public class TemporalDisplacement : CharacterModifier
{   
    public int radius = 3;
    private TileManager tileMgr;
    private CombatSequencer combatSeq;

    public TemporalDisplacement(CharacterSheet affectedCharacter, int radius = 3)
    {

        this.descriptors.Add(ModifierDescriptor.OneShot);
        this.descriptors.Add(ModifierDescriptor.Debuff);
        this.descriptors.Add(ModifierDescriptor.NoVisual);

        this.affectedCharacter = affectedCharacter;
        this.radius = radius;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/TemporalDisplacement");

        GameObject managers = GameObject.Find("System Managers");

        tileMgr = managers.GetComponent<TileManager>();
        combatSeq = managers.GetComponent<CombatSequencer>();
    }


    public override void StartEffect()
    {

        TeleportToRandomTile();
        combatSeq.PruneCombatBuffer(affectedCharacter.gameObject);
    }

    public override string GetDescription()
    {
        return "Teleport target to a random tile.";
    }

    private void TeleportToRandomTile()
    { 

        HashSet<Vector2Int> coordsInRadius = GameFunctions.GetCircleCoords(affectedCharacter.loc.coord, radius);

        // We only want coordinates inside the radius and not occupied.
        List<Vector2Int> validCoords = new();

        foreach (Vector2Int coord in coordsInRadius)
        {
            if (!tileMgr.levelCoords.Contains(coord))
            {
                continue;
            }

            if (tileMgr.occupiedlist.Contains(coord))
            {
                continue;
            }

            // Exclude current location, because the target is on it.
            if (coord == affectedCharacter.loc.coord)
            {
                continue;
            }

            validCoords.Add(coord);
        }

        if (validCoords.Count == 0)
        {
            return;
        }

        Vector2Int destination = validCoords[Random.Range(0, validCoords.Count)];
        affectedCharacter.Teleport(destination);
    }
}
