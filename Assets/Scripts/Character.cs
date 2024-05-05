using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    //pos is a position in 3d space
    public Vector3 pos;
    //coord is a position in 2d space from a top down perspective
    public Vector2Int coord;
    public int maxHealth = 100;
    public int health;
    public int accuracy = 75;
    public int minDamage = 50;
    public int maxDamage = 100;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        transform.position = pos;
        GetComponent<MoveToTarget>().target = pos;
        coord = new Vector2Int((int)pos.x, (int)pos.z);
    }

    public void Attack(Character target)
    {

        //turn towards target
        transform.rotation = Quaternion.Euler(0, DetermineRotation(pos, target.pos), 0);

        //roll to see if you hit
        if(Random.Range(0, 100) <= accuracy)
        {

            //roll damage
            target.TakeDamage(Random.Range(minDamage, maxDamage + 1));
        
        }else{

            GetComponent<TextPopup>().CreatePopup(transform.position, 2f, "Miss", Color.white);
        }
    }

    public int TakeDamage(int damage)
    {
        
        GetComponent<TextPopup>().CreatePopup(transform.position, 3f, damage.ToString(), Color.red);
        return health -= damage;
    }

    public bool Move(Vector3 newPos, HashSet<Vector3> occupiedlist)
    {
        if(!occupiedlist.Contains(newPos))
        {

            occupiedlist.Add(newPos);
            occupiedlist.Remove(pos);

            GetComponent<MoveToTarget>().SetTarget(newPos);
            transform.rotation = Quaternion.Euler(0, DetermineRotation(pos, newPos), 0);

            pos = newPos;
            coord = new Vector2Int((int)newPos.x, (int)newPos.z);

            return true;
        }
        
        return false;             
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



