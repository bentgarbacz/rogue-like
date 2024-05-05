using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    private bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 

        if ( (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I)) && isOpen == false)
        {
            isOpen = true;

        }else{
            
            isOpen = false;
        }
        
    }
}
