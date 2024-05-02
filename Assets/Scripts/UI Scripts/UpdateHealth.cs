using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpdateHealth : MonoBehaviour
{

    public TextMeshProUGUI healthStatus;
    public GameObject hero;

    // Update is called once per frame
    void Update()
    {
        Character c = hero.GetComponent<Character>();
        healthStatus.SetText(c.health.ToString() + " / " + c.maxHealth.ToString());
        //healthStatus.text = "why";
    }
}
