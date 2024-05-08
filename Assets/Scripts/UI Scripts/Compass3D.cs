using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass3D : MonoBehaviour
{
    public Vector3 NorthDir;
    public Transform Player; // Camera

    public Vector3 offsetFromPlayer; //needed to keep position from camera. this basically determines where on screen the compass will be positioned. 0,0,0 would be at the same position as the camera, experiment with other values to find one which positions compass relative to camera in such way that it displays on screen where you want it

    public GameObject NorthLayer;

    void Start()
    {
      //northDir can be whatever you want, I'm going to assume you want it to point along Unity's forward axis
      NorthDir = Vector3.forward;

      //set the compass to point to north (assuming its "N" needle points in the direction of model's forward axis (z+). if not, change the model, or nest it into a gameobject within which you'll rotate the model so that the N points along the parent gameobject's z+ axis
      NorthLayer.transform.rotation = Quaternion.LookRotation(NorthDir, Vector3.up);
      //nothing else needed, the rotation will be fine forever now. if you want to change it later, just do the above line again, and supply it the new NorthDir
    }


    // Update is called once per frame
    void Update()
    {
      //only thing you need to do in update, is keep the relative position to camera. tbh, easiest way would be to parent the compass, but you said you don't want that, so...
      transform.position = Player.position + (Player.rotation * offsetFromPlayer);
      //you need to make sure that the compass' offset from camera rotates with the direction of the camera itself, otherwise when camera rotates, the compass will get out of view. that's what multiplying by Player.rotation is there for. you rotate Vectors by multiplying them with quaternions (which is how we express rotations). 
    }
}