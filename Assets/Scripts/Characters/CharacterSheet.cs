using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MoveToTarget))]
public class CharacterSheet : MonoBehaviour
{

    public ObjectLocation loc;
    public CharacterHealth characterHealth;
    public int maxHealth = 1;
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
    protected StatusEffectManager statusEffectMgr;
    protected TileManager tileMgr;
    protected EntityManager entityMgr;
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip missClip;
    public string title = "N/A";
    protected GameObject managers;

    public virtual void Awake()
    {

        managers = GameObject.Find("System Managers");
        tileMgr = managers.GetComponent<TileManager>();
        entityMgr = managers.GetComponent<EntityManager>();

        GetComponent<MoveToTarget>().target = transform.position;
        loc.coord = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        characterHealth.InitHealth(maxHealth);

        statusEffectMgr = gameObject.AddComponent<StatusEffectManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
        missClip = Resources.Load<AudioClip>("Sounds/Miss");
    }

    public virtual bool Move(Vector2Int newCoord, float waitTime = 0f)
    {

        if (!tileMgr.occupiedlist.Contains(newCoord))
        {

            Vector3 newPos = new((float)newCoord.x, 0.1f, (float)newCoord.y);

            tileMgr.MoveEntity(this.gameObject, loc.coord, newCoord);

            loc.coord = newCoord;

            GetComponent<MoveToTarget>().SetTarget(newPos, waitTime);
            transform.rotation = Quaternion.Euler(0, GameFunctions.DetermineRotation(transform.position, newPos), 0);

            return true;
        }
        
        return false;             
    }

    public bool Teleport(Vector2Int newCoord)
    {

        if(!tileMgr.occupiedlist.Contains(newCoord))
        {

            Vector3 newPos = new((float)newCoord.x, 0.1f, (float)newCoord.y); 

            tileMgr.MoveEntity(this.gameObject, loc.coord, newCoord);
            
            transform.position = newPos;

            loc.coord = newCoord;

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

        if(statusEffectMgr == null)
        {

            return;
        }
        
        statusEffectMgr.ProcessStatusEffects();
    }
}




