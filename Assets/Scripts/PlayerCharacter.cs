using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 newPos, HashSet<Vector3> occupiedlist)
    {
        if(!occupiedlist.Contains(newPos))
        {

            occupiedlist.Add(newPos);
            occupiedlist.Remove(pos);

            transform.position = newPos;
            transform.rotation = Quaternion.Euler(0, DetermineRotation(pos, newPos), 0);
            pos = newPos;
        }
        
    }

    private float DetermineRotation(Vector3 start, Vector3 end)
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



