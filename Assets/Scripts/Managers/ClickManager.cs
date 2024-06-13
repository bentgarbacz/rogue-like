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
}




