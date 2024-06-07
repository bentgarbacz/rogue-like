using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpNotification : MonoBehaviour
{
    
    public GameObject hero;
    private UIActiveManager uiam;
    private PlayerCharacter playerCharacter;

    void Start()
    {
        
        playerCharacter = hero.GetComponent<PlayerCharacter>();
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    void Update()
    {
        
        if(playerCharacter.freeStatPoints > 0)
        {

            uiam.ShowLevelUpNotifications();

        }else
        {

            uiam.HideLevelUpNotifications();
        }
    }
}
