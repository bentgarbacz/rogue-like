using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    private EntityManager entityMgr;
    private TileManager tileMgr;
    private bool isOpen = false;
    private AudioSource audioSource;
    private bool toggleable = true;
    private ObjectHighlighter objectHighlighter;
    private BoxCollider doorBox;
    private BoxCollider visionBlockBox;

    void Awake()
    {

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        tileMgr = managers.GetComponent<TileManager>();

        audioSource = GetComponent<AudioSource>();
        objectHighlighter = GetComponent<ObjectHighlighter>();

        objectHighlighter.SetActionText("Open");
        
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        doorBox = colliders[0];
        visionBlockBox = colliders[1];
    }

    public override bool Interact()
    {

        if(ToggleDoor())
        {

            return true;
        }

        return false;
    }

    public void InitDoor(Vector2Int coord, Vector2Int orientation)
    {

        float angle = 0f;

        if (orientation.x == 1)
        {

            angle = 270f; // East facing door
            transform.position = new Vector3(transform.position.x + 0f, transform.position.y, transform.position.z + 0f);
        }
        else if (orientation.x == -1)
        {

            angle = 90f; // West facing door
            transform.position = new Vector3(transform.position.x + 0.95f, transform.position.y, transform.position.z + 0.95f);
        }
        else if (orientation.y == 1)
        {

            angle = 180f; // North facing door
            transform.position = new Vector3(transform.position.x + 0.95f, transform.position.y, transform.position.z + 0f);
        }
        else if (orientation.y == -1)
        {

            angle = 0f; // South facing door
            transform.position = new Vector3(transform.position.x + 0f, transform.position.y, transform.position.z + 0.95f);
        }

        transform.Rotate(0f, angle, 0f);
        this.loc.coord = coord;
        tileMgr.occupiedlist.Add(coord);
    }

    public bool ToggleDoor()
    {

        if(toggleable)
        {

            StartCoroutine(ToggleDelay());

            if (isOpen)
            {

                return CloseDoor();

            }else
            {

                return OpenDoor();
            }
        }

        return false;
    }

    private bool OpenDoor()
    {

        objectHighlighter.SetActionText("Close");
        audioSource.Play();
        isOpen = true;
        transform.Rotate(0f, 90f, 0f);
        tileMgr.occupiedlist.Remove(loc.coord);

        visionBlockBox.enabled = false;
        Physics.SyncTransforms();

        entityMgr.playerCharacter.RevealAroundPC();

        return true;
    }

    private bool CloseDoor()
    {

        if(!tileMgr.occupiedlist.Contains(loc.coord))
        {
            
            objectHighlighter.SetActionText("Open");
            audioSource.Play();
            isOpen = false;
            transform.Rotate(0f, -90f, 0f);
            tileMgr.occupiedlist.Add(loc.coord); 

            visionBlockBox.enabled = true;

            return true;
        }

        return false;
    }

    private IEnumerator ToggleDelay()
    {

        toggleable = false;
        yield return new WaitForSeconds(0.25f);
        toggleable = true;
    }   
}
