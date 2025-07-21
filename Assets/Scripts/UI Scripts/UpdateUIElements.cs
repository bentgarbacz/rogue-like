using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UpdateUIElements : MonoBehaviour
{

    public TextMeshProUGUI healthStatusHUD;
    public Image healthBar;
    public Image hungerBar;
    public Image xpBar;
    public TextMeshProUGUI manaStatusHUD;
    public Image manaBar;
    public TextMeshProUGUI healthStatus;
    public TextMeshProUGUI hungerStatus;
    public TextMeshProUGUI experienceStatus;
    public TextMeshProUGUI manaStatus;
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
    private PlayerCharacterSheet playerCharacter;
    private PlayerHealth playerHealth;
    private UIActiveManager uiam;

    void Start()
    {

        playerCharacter = hero.GetComponent<PlayerCharacterSheet>();
        playerHealth = hero.GetComponent<PlayerHealth>();
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    public void RefreshUI()
    {

        if(playerCharacter)
        {

            healthStatusHUD.SetText(playerHealth.currentHealth.ToString() + " / " + playerHealth.maxHealth.ToString());
            healthBar.fillAmount = (float)playerHealth.currentHealth / (float)playerHealth.maxHealth;
            hungerBar.fillAmount = (float)playerCharacter.hunger / (float)playerCharacter.maxHunger;
            xpBar.fillAmount = (float)playerCharacter.totalXP / (float)playerCharacter.GetCurrentLevelUpBreakpoint();
            manaStatusHUD.SetText(playerCharacter.mana.ToString() + " / " + playerCharacter.maxMana.ToString());
            manaBar.fillAmount = (float)playerCharacter.mana / (float)playerCharacter.maxMana;
 

            healthStatus.SetText("Health: " + playerHealth.currentHealth.ToString() + " / " + playerHealth.maxHealth.ToString());
            hungerStatus.SetText("Hunger: " + playerCharacter.hunger.ToString());
            experienceStatus.SetText("Experience: " + playerCharacter.totalXP.ToString() + " / " + playerCharacter.GetCurrentLevelUpBreakpoint().ToString());
            manaStatus.SetText("Mana: " + playerCharacter.mana.ToString() + " / " + playerCharacter.maxMana.ToString());

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

        SetLevelupNotification();
    }

    public void SetLevelupNotification()
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
