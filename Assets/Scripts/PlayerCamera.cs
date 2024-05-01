using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject focalPoint;
    public float panSpeed = 2f;
    private Vector3 localRotation;
    public float minYBound = 20f;
    public float maxYBound = 40f;
    public float minFov = 10f;
    public float maxFov = 40f;
    public float zoomSensitivity = 20f;

    void Start()
    {

        RotateCamera();
    }

    void LateUpdate()
    {

        if(focalPoint != null){

            transform.position = focalPoint.transform.position + new Vector3(0,0,0);

            if(Input.GetMouseButton(1)){

                RotateCamera();
            }

            float fov = Camera.main.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
        }
    }

    void RotateCamera()
    {      

        localRotation.x += Input.GetAxis("Mouse X") * panSpeed;
        localRotation.y += Input.GetAxis("Mouse Y") * panSpeed;

        localRotation.y = Mathf.Clamp(localRotation.y, minYBound, maxYBound);

        Quaternion QT = Quaternion.Euler(0f, localRotation.x, -localRotation.y);
        transform.rotation = QT;
    }

    public void setFocalPoint(GameObject focalPoint){

        this.focalPoint = focalPoint;  
    }
}

