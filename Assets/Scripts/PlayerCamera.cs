using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject focalPoint;
    public float speed = 2f;
    private Vector3 localRotation;

    void LateUpdate()
    {

        if(focalPoint != null){

            transform.position = focalPoint.transform.position;

            if(Input.GetMouseButton(1)){
                RotateCamera();
            }        
        }
    }

    void RotateCamera()
    {      

        localRotation.x += Input.GetAxis("Mouse X") * speed;
        localRotation.y += Input.GetAxis("Mouse Y") * speed;

        localRotation.y = Mathf.Clamp(localRotation.y, 20f, 80f);

        Quaternion QT = Quaternion.Euler(0f, localRotation.x, -localRotation.y);
        transform.rotation = QT;
    }

    public void setFocalPoint(GameObject focalPoint){

        this.focalPoint = focalPoint;  
    }
}

