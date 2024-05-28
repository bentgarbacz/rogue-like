using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggle : MonoBehaviour
{
    
    private UIActiveManager uiam;
    
    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    public void Click()
    {

        uiam.TogglePause();
    }
}
