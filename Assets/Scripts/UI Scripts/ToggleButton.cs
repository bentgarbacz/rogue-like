using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{

    public GameObject panel;
    public bool state = false;

    void Start()
    {

        panel.SetActive(state);
    }

    public void Click()
    {

        state = !state;
        panel.SetActive(state);
    }

}
