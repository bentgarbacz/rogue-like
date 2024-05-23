using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggle : MonoBehaviour
{
    
    private HotKeys hk;
    
    void Start()
    {

        hk = GameObject.Find("CanvasHUD").GetComponent<HotKeys>();;
    }

    public void Click()
    {

        hk.TogglePause();
    }
}
