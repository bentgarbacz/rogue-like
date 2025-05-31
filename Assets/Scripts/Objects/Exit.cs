using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Interactable
{

    private DungeonManager dum;
    private LevelGenerator lg;

    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        lg = GameObject.Find("Map Generator").GetComponent<LevelGenerator>();
    }

    public override bool Interact()
    {
                             
        lg.NewLevel(lg.biomeDict[BiomeType.Catacomb]);
        
        return true;
    }
}
