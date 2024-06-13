using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineOfSight
{

    private static readonly LayerMask layerMask =  1 << LayerMask.NameToLayer("Line of Sight Targets"); // Line of Sight targets is a layer that contains every object involved with calculating line of sight

    public static bool HasLOS(GameObject startObject, GameObject targetObject)
    {

        Vector3 direction = targetObject.transform.position - startObject.transform.position;

        float distance = direction.magnitude; // Distance between the objects
        direction.Normalize(); // Normalize the direction vector

        if (Physics.Raycast(startObject.transform.position, direction, out RaycastHit hit, distance, layerMask))
        {

            if(hit.transform.gameObject == targetObject)
            {

                //If the raycast hits the specified target, the path is not obstructed
                return true;
            }

            // If the raycast hits something within the specified distance and layer mask, the path is obstructed
            return false;
        }

        //return true if no obstructions are hit
        //This is necessary because depending on the hitbox of the target, the raycast may not hit it
        return true;
    }
}
