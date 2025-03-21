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
    private float fov = 0;
    [SerializeField] GameObject mapPanel;

    void Start()
    {

        fov = Camera.main.fieldOfView;
        RotateCamera();
    }

    void LateUpdate()
    {

        if (focalPoint != null) {
            
            //follow player on x and z axes, not y axis
            if(!focalPoint.GetComponent<AttackAnimation>().IsAttacking())
            {

                transform.position = new Vector3(focalPoint.transform.position.x, 0, focalPoint.transform.position.z);
            }

            
            if (Input.GetMouseButton(1)) {

                RotateCamera();
            }

            if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                
                ZoomCamera();
            }
        }
    }

    void RotateCamera()
    {      

        localRotation.x += Input.GetAxis("Mouse X") * panSpeed;
        localRotation.y += Input.GetAxis("Mouse Y") * panSpeed;

        localRotation.y = Mathf.Clamp(localRotation.y, minYBound, maxYBound);

        Quaternion QT = Quaternion.Euler(0f, localRotation.x, -localRotation.y);
        transform.rotation = QT;
        mapPanel.transform.rotation = Quaternion.Euler(0f, 0f, localRotation.x - 90f);
    }

    void ZoomCamera()
    {

        fov -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    public void SetFocalPoint(GameObject focalPoint){

        this.focalPoint = focalPoint;  
    }
}

