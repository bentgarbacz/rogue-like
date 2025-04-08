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
    //[SerializeField] GameObject mapPanel;
    [SerializeField] Camera MinimapCamera;

    void Start()
    {

        fov = Camera.main.fieldOfView;
        RotateCamera(Input.GetAxis("Mouse X") * panSpeed, Input.GetAxis("Mouse Y") * panSpeed);
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

                RotateCamera(Input.GetAxis("Mouse X") * panSpeed, Input.GetAxis("Mouse Y") * panSpeed);
            }

            if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                
                ZoomCamera(Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity);
            }
        }
    }

    public void RotateCamera(float xChange, float yChange)
    {      

        localRotation.x += xChange;
        localRotation.y += yChange;

        localRotation.y = Mathf.Clamp(localRotation.y, minYBound, maxYBound);

        transform.rotation = Quaternion.Euler(0f, localRotation.x, -localRotation.y);
        MinimapCamera.transform.rotation = Quaternion.Euler(270f, -(localRotation.x - 90f), 0f);
    }

    public void ZoomCamera(float fovChange)
    {

        fov -= fovChange;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    public void SetFocalPoint(GameObject focalPoint){

        this.focalPoint = focalPoint;  
    }
}

