using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Interactable
{

    private LevelGenerator levelGenerator;
    public BiomeType targetBiome = BiomeType.Catacomb;

    void Start()
    {

        levelGenerator = GameObject.Find("Map Generator").GetComponent<LevelGenerator>();
    }

    public override bool Interact()
    {
                             
        levelGenerator.NewLevel(levelGenerator.biomeDict[targetBiome]);
        
        return true;
    }

    public void SetTargetBiome(BiomeType biome)
    {
        targetBiome = biome;
    }
}
