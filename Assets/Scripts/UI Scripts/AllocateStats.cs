using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllocateStats : MonoBehaviour
{

    public AudioSource audioSource;
    public string statType = "";
    public GameObject hero;
    private PlayerCharacterSheet playerCharacter;
    private InventoryManager im;

    void Awake()
    {

        if(hero)
        {
            
            playerCharacter = hero.GetComponent<PlayerCharacterSheet>();
        }

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
    }

    public void Click()
    {

        if(playerCharacter)
        {
            
            audioSource.Play();

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

            im.UpdateStats();
            playerCharacter.UpdateUI();
        }
    }
}
