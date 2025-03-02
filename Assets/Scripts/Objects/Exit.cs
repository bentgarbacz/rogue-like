using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{

    public Vector2Int coord;
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
