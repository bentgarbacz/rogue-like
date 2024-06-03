using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{

    private AudioSource audioSource;

     void Start()
    {

        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
    }

    public void Click()
    {

        audioSource.Play();
        Application.Quit();
    }
}
