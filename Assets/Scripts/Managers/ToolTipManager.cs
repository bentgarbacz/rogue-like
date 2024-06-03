using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
/* 
    Camera cam;
    Vector3 min, max;
    RectTransform rect;
    float offset = 10f;
    private UIActiveManager uiam;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();

        cam = Camera.main;
        rect = GetComponent<RectTransform>();
        min = new Vector3(0, 0, 0);
        max = new Vector3(cam.pixelWidth, cam.pixelHeight, 0);
    }

    void Update()
    {

        if (uiam.toolTipContainerIsOpen)
        {
            //get the tooltip position with offset
            Vector3 position = new Vector3(Input.mousePosition.x + rect.rect.width, Input.mousePosition.y - (rect.rect.height / 2 + offset), 0f);
            //clamp it to the screen size so it doesn't go outside
            transform.position = new Vector3(Mathf.Clamp(position.x, min.x + rect.rect.width/2, max.x - rect.rect.width/2), Mathf.Clamp(position.y, min.y + rect.rect.height / 2, max.y - rect.rect.height / 2), transform.position.z);
        }
            
    }

    public void SetToolTip(bool status)
    {

        if(status)
        {

            uiam.ShowToolTip();

        }else if(!status)
        {

            uiam.HideToolTip();
        }
    } */
}
