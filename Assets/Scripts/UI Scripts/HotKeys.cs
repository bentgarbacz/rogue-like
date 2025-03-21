using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeys : MonoBehaviour
{

    private UIActiveManager uiam;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            uiam.TogglePause();

        }else if(Input.GetKeyDown(KeyCode.I))
        {
            
            uiam.ToggleInventory();

        }else if(Input.GetKeyDown(KeyCode.C))
        {

            uiam.ToggleCharacter();

        }
        else if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Tab))
        {

            uiam.ToggleMap();
        }
    }
    
}
