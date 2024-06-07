using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    
    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    public GameObject GetObject()
    {

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
    
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {   

            return hit.transform.gameObject;
        }

        return null;
    }
/*     public GameObject GetObject(Mouse mouse)
    {
        print("yo");
        Vector3 mousePosition = mouse.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        //returns GameObject hit by ray
        if (Physics.Raycast(ray, out RaycastHit hit))
        {            

            return hit.collider.gameObject;
        }

        return null;
    } */
}




