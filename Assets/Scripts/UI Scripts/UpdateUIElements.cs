using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpdateHealth : MonoBehaviour
{

    public TextMeshProUGUI healthStatus;
    public TextMeshProUGUI hungerStatus;
    public TextMeshProUGUI xpStatus;
    public TextMeshProUGUI levelStatus;
    public GameObject hero;
    private PlayerCharacter playerCharacter;

    void Start()
    {

        playerCharacter = hero.GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        if(playerCharacter)
        {

            healthStatus.SetText(playerCharacter.health.ToString() + " / " + playerCharacter.maxHealth.ToString());
            hungerStatus.SetText(playerCharacter.hunger.ToString());
            xpStatus.SetText(playerCharacter.totalXP.ToString() + " / " + playerCharacter.levelUpBreakpoints[0].ToString());
            levelStatus.SetText(playerCharacter.level.ToString());

        }else
        {

            healthStatus.SetText("0");
        }
        
    }
}
