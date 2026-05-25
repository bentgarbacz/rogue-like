using System.Collections.Generic;
using UnityEngine;

public class TemporalDisplacement : CharacterModifier
{   
    public int radius = 3;
    private TileManager tileMgr;

    public TemporalDisplacement(CharacterSheet affectedCharacter, int radius = 3)
    {

        this.descriptors.Add(ModifierDescriptor.Temporary);
        this.descriptors.Add(ModifierDescriptor.Debuff);
        this.descriptors.Add(ModifierDescriptor.NoVisual);

        this.affectedCharacter = affectedCharacter;
        this.radius = radius;
        this.duration = 1;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/TemporalDisplacement");

        tileMgr = GameObject.Find("System Managers").GetComponent<TileManager>();
    }


    public override int Effect()
    {

        TeleportToRandomTile();

        duration -= 1;
        return duration;
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
