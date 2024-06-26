using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public int hunger;
    public int maxHunger = 1000;
    private int hungerBuffer = 0;
    public int totalXP = 0;
    public int levelUpBreakpoint = 50;
    public int freeStatPoints = 0;
    public AudioClip stepAudioClip;
    public AudioClip levelUpAudioClip;
    public int accuracyBonus = 0;
    public int minDamageBonus = 0;
    public int maxDamageBonus = 0;
    public int speedBonus = 0;
    public int critChanceBonus = 0;
    public int critMultiplierBonus = 0;
    public int vitalityBonus = 0;
    public int strengthBonus = 0;
    public int dexterityBonus = 0;
    public int intelligenceBonus = 0;
    public int armorBonus = 0;
    public int evasionBonus = 0;
    public int maxHealthBonus = 0;
    

    public override void Start()
    {
        
        base.Start();
        maxHealth = 20;
        health = maxHealth;
        accuracy = 1000;
        minDamage = 1;
        maxDamage = 3;
        level = 1;
        speed = 10;
        hunger = maxHunger;
        armor = 0;
        evasion = 50;
        
        attackClip = Resources.Load<AudioClip>("Sounds/Strike");
        stepAudioClip = Resources.Load<AudioClip>("Sounds/Step");
        levelUpAudioClip = Resources.Load<AudioClip>("Sounds/LevelUp");

        GetComponent<MoveToTarget>().SetNoise(audioSource, stepAudioClip);
    }

    public void GainXP(int XP)
    {

        totalXP += XP;

        if(totalXP >= levelUpBreakpoint)
        {

            GetComponent<TextPopup>().CreatePopup(transform.position, 3f, "Level Up!", Color.yellow);

            while(totalXP >= levelUpBreakpoint)
            {

                LevelUp();
                totalXP -= levelUpBreakpoint;
                levelUpBreakpoint += ((2 * levelUpBreakpoint) / 3);
            }

        }else
        {

            GetComponent<TextPopup>().CreatePopup(transform.position, 2f, XP.ToString() + " XP", Color.green);
        }
    }

    public void LevelUp()
    {
        
        audioSource.PlayOneShot(levelUpAudioClip);

        Heal(maxHealth / 4);
        freeStatPoints += 5;

        level += 1;
    }

    public int GetCurrentLevelUpBreakpoint()
    {

        return levelUpBreakpoint;
    }

    public void SatiateHunger(int hungerValue)
    {

        hunger = System.Math.Min(maxHunger, hunger + hungerValue);
    }

    public void BecomeHungrier(int hungerValue = 1)
    {

        hunger -= hungerValue;
        hungerBuffer += 1;

        if(hungerBuffer >= 10)
        {

            Heal(1);
            hungerBuffer = 0;
        }
    }
}