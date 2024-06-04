using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpdateUIElements : MonoBehaviour
{

    public TextMeshProUGUI healthStatusHUD;
    public TextMeshProUGUI hungerStatusHUD;
    public TextMeshProUGUI xpStatusHUD;
    public TextMeshProUGUI levelStatusHUD;
    public TextMeshProUGUI healthStatus;
    public TextMeshProUGUI hungerStatus;
    public TextMeshProUGUI experienceStatus;
    public TextMeshProUGUI strengthStatus;
    public TextMeshProUGUI dexterityStatus;
    public TextMeshProUGUI intelligenceStatus;
    public TextMeshProUGUI speedStatus;
    public TextMeshProUGUI critChanceStatus;
    public TextMeshProUGUI armorStatus;
    public TextMeshProUGUI evasionStatus;
    public TextMeshProUGUI accuracyStatus;
    public TextMeshProUGUI damageRangeStatus;
    public TextMeshProUGUI freeStatPointsStatus;

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

            healthStatusHUD.SetText(playerCharacter.health.ToString() + " / " + playerCharacter.maxHealth.ToString());
            hungerStatusHUD.SetText(playerCharacter.hunger.ToString());
            xpStatusHUD.SetText(playerCharacter.totalXP.ToString() + " / " + playerCharacter.levelUpBreakpoints[0].ToString());
            levelStatusHUD.SetText(playerCharacter.level.ToString());

            healthStatus.SetText("Health: " + playerCharacter.health.ToString() + " / " + playerCharacter.maxHealth.ToString());
            hungerStatus.SetText("Hunger: " + playerCharacter.hunger.ToString());
            experienceStatus.SetText("Experience: " + playerCharacter.totalXP.ToString() + " / " + playerCharacter.levelUpBreakpoints[0].ToString());

            strengthStatus.SetText("Strength: " + playerCharacter.strength.ToString());
            dexterityStatus.SetText("Dexterity: " + playerCharacter.dexterity.ToString());
            intelligenceStatus.SetText("Intelligence: " + playerCharacter.intelligence.ToString());
            freeStatPointsStatus.SetText("Available Stat Points: " + playerCharacter.freeStatPoints.ToString());

            speedStatus.SetText("Speed: " + playerCharacter.speed.ToString());
            critChanceStatus.SetText("Crit Chance: " + playerCharacter.critChance.ToString());
            accuracyStatus.SetText("Accuracy: " + playerCharacter.accuracy.ToString());

            armorStatus.SetText("Armor: " + playerCharacter.armor.ToString());
            evasionStatus.SetText("Evasion: " + playerCharacter.evasion.ToString());

            damageRangeStatus.SetText("Damage: " + playerCharacter.minDamage.ToString() + " - " + playerCharacter.maxDamage.ToString());
            













            

        }else
        {

            healthStatus.SetText("0");
        }
        
    }
}
