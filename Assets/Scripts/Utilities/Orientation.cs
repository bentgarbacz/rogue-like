
using UnityEngine;

public static class Orientation
{
    
    public static float DetermineRotation(Vector3 start, Vector3 end)
    {
        
        if(start.x == end.x && start.z < end.z)
        {
            return 0f; //north
        }
        else if(start.x < end.x && start.z < end.z)
        {
            return 45f; //north east
        }
        else if(start.x < end.x && start.z == end.z)
        {
            return 90f; // east

        }else if(start.x < end.x && start.z > end.z)
        {
            return 135f; //south east

        }else if(start.x == end.x && start.z > end.z)
        {
            return 180f; // south

        }else if(start.x > end.x && start.z > end.z)
        {
            return 225f; //south west

        }else if(start.x > end.x && start.z == end.z)
        {
            return 270f; // west
            
        }else if(start.x > end.x && start.z < end.z)
        {
            return 315f; //north west
        }

        return 0f;
    }
}
