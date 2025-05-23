using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet : MonoBehaviour
{

    //pos is a position in 3d space
    public Vector3 pos;
    //coord is a position in 2d space from a top down perspective
    public Vector2Int coord;
    public int maxHealth = 1;
    public int health = 1;
    public int accuracy = 100;
    public int minDamage = 0;
    public int maxDamage = 0;
    public int level = 0;
    public int speed;
    public int critChance;
    public int critMultiplier = 2;
    public int vitality = 0;
    public int strength = 0;
    public int dexterity = 0;
    public int intelligence = 0;
    public int armor = 0;
    public int evasion = 0;    
    public string dropTable;
    protected StatusEffectManager sem;
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip missClip;
    public string title = "N/A";

    public virtual void Start()
    {
        
        transform.position = pos;
        GetComponent<MoveToTarget>().target = pos;
        coord = new Vector2Int((int)pos.x, (int)pos.z);

        sem = gameObject.AddComponent<StatusEffectManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
        missClip = Resources.Load<AudioClip>("Sounds/Miss");
    }

    public virtual int TakeDamage(int damage)
    {

        return health -= damage;
    }

    public virtual void Heal(int healValue)
    {

        health = System.Math.Min(maxHealth, health + healValue);
    }

    public virtual bool Move(Vector2Int newCoord, HashSet<Vector2Int> occupiedlist, float waitTime = 0f)
    {

        if(!occupiedlist.Contains(newCoord))
        {

            Vector3 newPos = new((float)newCoord.x, 0.1f, (float)newCoord.y); 

            occupiedlist.Add(newCoord);
            occupiedlist.Remove(coord);

            GetComponent<MoveToTarget>().SetTarget(newPos, waitTime);
            transform.rotation = Quaternion.Euler(0, GameFunctions.DetermineRotation(pos, newPos), 0);

            pos = newPos;
            coord = new Vector2Int((int)newPos.x, (int)newPos.z);
            coord = newCoord;

            return true;
        }
        
        return false;             
    }

    public bool Teleport(Vector2Int newCoord, DungeonManager dum)
    {

        if(!dum.occupiedlist.Contains(newCoord))
        {

            Vector3 newPos = new((float)newCoord.x, 0.1f, (float)newCoord.y); 

            dum.occupiedlist.Add(newCoord);
            dum.occupiedlist.Remove(coord);
            
            transform.position = newPos;

            pos = newPos;
            coord = newCoord;

            return true;

        }

        return false;
    }

    public string GetName()
    {

        return title;
    }

    public virtual void ProcessStatusEffects()
    {

        if(sem == null)
        {

            return;
        }
        
        sem.ProcessStatusEffects();
    }
}




