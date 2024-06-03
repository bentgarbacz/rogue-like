using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickManager : MonoBehaviour
{
    
    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    public GameObject getObject(Mouse mouse)
    {

        Vector3 mousePosition = mouse.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        //returns GameObject hit by ray
        if (Physics.Raycast(ray, out RaycastHit hit))
        {            

            return hit.collider.gameObject;
        }

        return null;
    }
}




