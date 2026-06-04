using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(MoveToTarget))]
public class CharacterSheet : MonoBehaviour
{

    public ObjectLocation loc;
    public CharacterHealth characterHealth;
    public CharacterStats stats;
    public int level = 0;
    public bool isActionBlocked = false;
    public DropTableType dropTable = DropTableType.None;
    public CharacterModifierManager characterModMgr;
    protected TileManager tileMgr;
    protected EntityManager entityMgr;
    protected LockManager lockMgr;
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip missClip;
    public string title = "N/A";
    protected GameObject managers;
    protected TextNotificationManager notificationManager;
    protected LogManager logMgr;

    public virtual void Awake()
    {

        managers = GameObject.Find("System Managers");
        tileMgr = managers.GetComponent<TileManager>();
        entityMgr = managers.GetComponent<EntityManager>();
        logMgr = logMgr = managers.GetComponent<UIActiveManager>().logPanel.GetComponent<LogManager>();

        lockMgr = GetComponent<LockManager>();
        notificationManager = GetComponent<TextNotificationManager>();

        GetComponent<MoveToTarget>().target = transform.position;
        loc.coord = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        missClip = Resources.Load<AudioClip>("Sounds/Miss");
    }

    public virtual bool Move(Vector2Int newCoord, float waitTime = 0f)
    {

        if (tileMgr.occupiedlist.Contains(newCoord))
        {

            return false;            
        }

        Vector3 newPos = new((float)newCoord.x, 0.1f, (float)newCoord.y);

        tileMgr.MoveEntity(this.gameObject, loc.coord, newCoord);

        loc.coord = newCoord;

        GetComponent<MoveToTarget>().SetTarget(newPos, waitTime);
        transform.rotation = Quaternion.Euler(0, GameFunctions.DetermineRotation(transform.position, newPos), 0);

        return true;
    
    }

    public virtual bool Teleport(Vector2Int newCoord)
    {

        if (tileMgr.occupiedlist.Contains(newCoord))
        {


            return false;
        }

        Vector3 newPos = new((float)newCoord.x, 0.1f, (float)newCoord.y);

        tileMgr.MoveEntity(this.gameObject, loc.coord, newCoord);

        transform.position = newPos;

        loc.coord = newCoord;

        return true;
    }

    public string GetName()
    {

        return title;
    }

    public virtual void OnDeath()
    {

        return;
    }

    public virtual void Die()
    {

        logMgr.CreateLogEntry(title + " has been slain", Color.grey);
        entityMgr.KillEntity(this.gameObject);
        OnDeath();
        Destroy(this.gameObject);
    }

    public virtual void OnDamage()
    {
        
        return;
    }
}