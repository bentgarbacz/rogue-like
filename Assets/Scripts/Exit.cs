using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Tile
{

    private DungeonManager dum;

    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
    }
    public void ExitLevel()
    {

        dum.CleanUp();
    }
}
