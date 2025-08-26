using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    private EntityManager entityMgr;

    public void Awake()
    {

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
    }

    public virtual int TakeDamage(int damage)
    {

        currentHealth = System.Math.Max(0, currentHealth - damage);

        if (currentHealth <= 0)
        {

            StartCoroutine(Die());
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

    private IEnumerator Die()
    {

        yield return new WaitForSeconds(0.05f); //Give the GameObject of dead character time to wrap up before it is destroyed
        entityMgr.KillEntity(this.gameObject);
    }
}
