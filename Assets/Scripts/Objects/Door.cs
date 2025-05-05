using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    private DungeonManager dum;
    private bool isOpen = false;
    private Vector3 pos;
    private AudioSource audioSource;
    private bool toggleable = true;
    private ObjectHighlighter objectHighlighter;

    void Start()
    {
        
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        audioSource = GetComponent<AudioSource>();
        objectHighlighter = GetComponent<ObjectHighlighter>();

        objectHighlighter.actionDescription = "Open";
    }

    public override void Interact()
    {

        ToggleDoor();
    }

    public void ToggleDoor()
    {

        if(toggleable)
        {

            StartCoroutine(ToggleDelay());

            if (isOpen)
            {

                CloseDoor();

            }else
            {

                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {

        objectHighlighter.actionDescription = "Close";
        audioSource.Play();
        isOpen = true;
        transform.Rotate(0f, 90f, 0f);
        dum.occupiedlist.Remove(coord); 
    }

    private void CloseDoor()
    {

        if(!dum.occupiedlist.Contains(coord))
        {
            
            objectHighlighter.actionDescription = "Open";
            audioSource.Play();
            isOpen = false;
            transform.Rotate(0f, -90f, 0f);
            dum.occupiedlist.Add(coord); 
        }
    }

    private IEnumerator ToggleDelay()
    {

        toggleable = false;
        yield return new WaitForSeconds(0.25f);
        toggleable = true;
    }   
}
