using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickManager : MonoBehaviour
{
    Camera m_Camera;
    void Awake()
    {
        m_Camera = Camera.main;
    }

    public GameObject getObject(Mouse mouse)
    {

        Vector3 mousePosition = mouse.position.ReadValue();
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {            
           
            return hit.collider.gameObject;
        }

        return null;
    }
}


