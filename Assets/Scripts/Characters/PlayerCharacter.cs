using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public int hunger;
    private int maxHunger = 1000;
    private int hungerBuffer = 0;
    public int totalXP = 0;
    public List<int> levelUpBreakpoints = new List<int>{20, 50, 90, 140, 200};

    public override void Start()
    {
        
        base.Start();
        maxHealth = 20;
        health = maxHealth;
        accuracy = 80;
        minDamage = 5;
        maxDamage = 10;
        level = 1;
        speed = 10;
        hunger = maxHunger;
    }

    public void GainXP(int XP)
    {

        totalXP += XP;

        if(totalXP >= levelUpBreakpoints[0])
        {

            GetComponent<TextPopup>().CreatePopup(transform.position, 3f, "Level Up!", Color.yellow);

            while(totalXP >= levelUpBreakpoints[0])
            {

                LevelUp();
                totalXP -= levelUpBreakpoints[0];
                levelUpBreakpoints.RemoveAt(0);
            }

        }else
        {

            GetComponent<TextPopup>().CreatePopup(transform.position, 2f, XP.ToString() + " XP", Color.green);
        }
    }

    public void LevelUp()
    {
        
        level += 1;
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