using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpdateHealth : MonoBehaviour
{

    public TextMeshProUGUI healthStatus;
    public GameObject hero;
    private Character playerCharacter;

    void Start()
    {

        playerCharacter = hero.GetComponent<Character>();
    }

    void Update()
    {
        if(playerCharacter)
        {

            healthStatus.SetText(playerCharacter.health.ToString() + " / " + playerCharacter.maxHealth.ToString());

        }else
        {

            healthStatus.SetText("0");
        }
        
    }
}
