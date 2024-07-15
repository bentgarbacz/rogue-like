using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    
    private UIActiveManager uiam;
    private AudioSource audioSource;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
    }

    public void ClickInventory()
    {

        audioSource.Play();
        uiam.ToggleInventory();        
    }

    public void ClickCharacter()
    {

        audioSource.Play();
        uiam.ToggleCharacter();        
    }

    public void ClickPause()
    {

        audioSource.Play();
        uiam.TogglePause();        
    }

    public void ClickAssignSpell()
    {

        audioSource.Play();
        uiam.HideAssignSpell();
    }
}
