using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    //pos is a position in 3d space
    public Vector3 pos;
    //coord is a position in 2d space from a top down perspective
    public Vector2Int coord;
    public int maxHealth = 1;
    public int health = 1;
    public int accuracy = 100;
    public int minDamage = 1;
    public int maxDamage = 1;
    public int level = 0;
    public string dropTable;


    // Start is called before the first frame update
    public virtual void Start()
    {
        
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
        
        }else
        {

            //take 0 damage on miss
            target.TakeDamage(0);
        }
    }

    public int TakeDamage(int damage)
    {
        
        if(damage > 0)
        {
            GetComponent<TextPopup>().CreatePopup(transform.position, 3f, damage.ToString(), Color.red);

        }else
        {

            GetComponent<TextPopup>().CreatePopup(transform.position, 2f, "Miss", Color.white);
        }
        return health -= damage;
    }

    public void Heal(int healValue)
    {

        health = System.Math.Min(maxHealth, health + healValue);
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

    public bool Teleport(Vector3 newPos, HashSet<Vector3> occupiedlist)
    {

        if(!occupiedlist.Contains(newPos))
        {
            occupiedlist.Add(newPos);
            occupiedlist.Remove(pos);
            
            transform.position = newPos;

            pos = newPos;
            coord = new Vector2Int((int)newPos.x, (int)newPos.z);

            return true;

        }

        return false;
    }

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



