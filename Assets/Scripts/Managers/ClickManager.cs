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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        int ignoreLayer = 8;
        int layerMask = ~(1 << ignoreLayer);
    
        if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {

            return hit.transform.gameObject;
        }

        return null;
    }
}




