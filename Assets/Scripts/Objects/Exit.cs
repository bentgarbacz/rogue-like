using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Interactable
{

    private LevelGenerator levelGenerator;

    void Start()
    {

        levelGenerator = GameObject.Find("Map Generator").GetComponent<LevelGenerator>();
    }

    public override bool Interact()
    {
                             
        levelGenerator.NewLevel(levelGenerator.biomeDict[BiomeType.Catacomb]);
        
        return true;
    }
}
