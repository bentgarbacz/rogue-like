using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterToggle : MonoBehaviour
{

    AudioSource audioSource;
    private UIActiveManager uiam;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
    }

    public void Click()
    {

        audioSource.Play();
        uiam.ToggleCharacter();
    }

}
