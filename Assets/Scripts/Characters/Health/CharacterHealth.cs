using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    private EntityManager entityMgr;
    private CharacterSheet characterSheet;

    public void Awake()
    {

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        characterSheet = GetComponent<CharacterSheet>();
    }

    public virtual int TakeDamage(int damage)
    {

        currentHealth = System.Math.Max(0, currentHealth - damage);

        if(damage > 0)
        {
            
            characterSheet.OnDamage();
        }

        if (currentHealth <= 0)
        {

            //StartCoroutine(Die());
            Die();
        }

        return currentHealth;
    }

    public virtual void Heal(int healValue)
    {
        if (healValue <= 0)
        {

            return;
        }

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

    private void Die()
    {

        entityMgr.KillEntity(this.gameObject);
        characterSheet.OnDeath();
        Destroy(this.gameObject);
    }
}
