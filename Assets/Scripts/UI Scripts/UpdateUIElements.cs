using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Threading;

public class UpdateUIElements : MonoBehaviour
{

    public TextMeshProUGUI healthStatusHUD;
    public Image healthBar;
    public Image hungerBar;
    public Image barrierBar;
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
    public TextMeshProUGUI barrierStatus;
    public TextMeshProUGUI freeStatPointsStatus;
    public GameObject hero;
    private PlayerCharacterSheet playerCharacter;
    private PlayerHealth playerHealth;
    private UIActiveManager uiam;

    void Awake()
    {

        playerCharacter = hero.GetComponent<PlayerCharacterSheet>();
        playerHealth = hero.GetComponent<PlayerHealth>();
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    public void RefreshUI()
    {

        if(playerCharacter)
        {

            healthStatusHUD.SetText(playerHealth.currentHealth.ToString() + " / " + playerCharacter.stats.maxHealth.ToString());
            healthBar.fillAmount = (float)playerHealth.currentHealth / (float)playerCharacter.stats.maxHealth;
            hungerBar.fillAmount = (float)playerCharacter.hunger / (float)playerCharacter.maxHunger;

            if(playerCharacter.stats.maxBarrier > 0)
            {

                barrierBar.fillAmount = (float)playerHealth.currentBarrier / (float)playerCharacter.stats.maxBarrier;
            }
            else
            {
                
                barrierBar.fillAmount = 0f;
            }
            
            xpBar.fillAmount = (float)playerCharacter.totalXP / (float)playerCharacter.GetCurrentLevelUpBreakpoint();
            manaStatusHUD.SetText(playerCharacter.mana.ToString() + " / " + playerCharacter.stats.maxMana.ToString());
            manaBar.fillAmount = (float)playerCharacter.mana / (float)playerCharacter.stats.maxMana;
 

            healthStatus.SetText("Health: " + playerHealth.currentHealth.ToString() + " / " + playerCharacter.stats.maxHealth.ToString());
            hungerStatus.SetText("Hunger: " + playerCharacter.hunger.ToString());
            experienceStatus.SetText("Experience: " + playerCharacter.totalXP.ToString() + " / " + playerCharacter.GetCurrentLevelUpBreakpoint().ToString());
            manaStatus.SetText("Mana: " + playerCharacter.mana.ToString() + " / " + playerCharacter.stats.maxMana.ToString());

            strengthStatus.SetText("Strength: " + playerCharacter.stats.strength.ToString());
            dexterityStatus.SetText("Dexterity: " + playerCharacter.stats.dexterity.ToString());
            intelligenceStatus.SetText("Intelligence: " + playerCharacter.stats.intelligence.ToString());
            freeStatPointsStatus.SetText("Available Stat Points: " + playerCharacter.freeStatPoints.ToString());

            speedStatus.SetText("Speed: " + playerCharacter.stats.speed.ToString());
            critChanceStatus.SetText("Crit Chance: " + playerCharacter.stats.critChance.ToString());
            accuracyStatus.SetText("Accuracy: " + playerCharacter.stats.accuracy.ToString());

            armorStatus.SetText("Armor: " + playerCharacter.stats.armor.ToString());
            evasionStatus.SetText("Evasion: " + playerCharacter.stats.evasion.ToString());

            damageRangeStatus.SetText("Damage: " + playerCharacter.stats.minDamage.ToString() + " - " + playerCharacter.stats.maxDamage.ToString());

            barrierStatus.SetText("Barrier: " + playerCharacter.stats.maxBarrier);

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
