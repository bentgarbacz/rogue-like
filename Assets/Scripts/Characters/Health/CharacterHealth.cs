using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    public virtual int TakeDamage(int damage)
    {

        return currentHealth = System.Math.Max(0, currentHealth - damage);
    }

    public virtual void Heal(int healValue)
    {

        currentHealth = System.Math.Min(maxHealth, currentHealth + healValue);
    }

    public void InitHealth(int maxHealth, int currentHealth = 0)
    {

        this.maxHealth = maxHealth;

        if (currentHealth > 0 && currentHealth <= maxHealth)
        {

            this.currentHealth = currentHealth;

        }
        else
        {

            this.currentHealth = maxHealth;
        }
    }
}
