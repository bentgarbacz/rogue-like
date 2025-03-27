using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeys : MonoBehaviour
{

    private UIActiveManager uiam;
    [SerializeField] private PlayerCamera playerCamera;

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
        
        if(Input.GetKey(KeyCode.UpArrow))
        {

            playerCamera.RotateCamera(0f, -1f);

        }else if(Input.GetKey(KeyCode.DownArrow))
        {

            playerCamera.RotateCamera(0f, +1f);
            
        }
        
        if(Input.GetKey(KeyCode.LeftArrow))
        {

            playerCamera.RotateCamera(1f, 0f);
            
        }else if(Input.GetKey(KeyCode.RightArrow))
        {

            playerCamera.RotateCamera(-1f, 0f);
        }

        if(Input.GetKey(KeyCode.Minus))
        {

            playerCamera.ZoomCamera(-0.25f);
            
        }else if(Input.GetKey(KeyCode.Equals))
        {

            playerCamera.ZoomCamera(0.25f);
        }
    }
    
}
