using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllocateStats : MonoBehaviour
{

    public string statType = "";
    public GameObject hero;
    private PlayerCharacter playerCharacter;

    void Start()
    {
        
        playerCharacter = hero.GetComponent<PlayerCharacter>();
    }

    public void Click()
    {

        if(statType == "Strength")
        {

            playerCharacter.freeStatPoints -= 1;
            playerCharacter.strength += 1;

        }else if(statType == "Dexterity")
        {

            playerCharacter.freeStatPoints -= 1;
            playerCharacter.dexterity += 1;

        }else if(statType == "Intelligence")
        {

            playerCharacter.freeStatPoints -= 1;
            playerCharacter.intelligence += 1;
        }
    }
}
