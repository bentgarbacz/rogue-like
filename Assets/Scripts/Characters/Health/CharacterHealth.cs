using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterHealth : MonoBehaviour
{

    public int currentHealth;
    public int currentBarrier;
    private int currentBarrierTimer = 0;
    protected EntityManager entityMgr;
    protected CharacterSheet characterSheet;

    public void Awake()
    {

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        characterSheet = GetComponent<CharacterSheet>();

        currentHealth = characterSheet.stats.maxHealth;
        currentBarrier = characterSheet.stats.maxBarrier;
    }

    public virtual void TakeDamage(int damage)
    {

        currentBarrierTimer = 0;

        currentBarrier -= damage;

        if(currentBarrier < 0)
        {
            
            damage = currentBarrier * -1;
            currentBarrier = 0;
        }
        else
        {
            
            damage = 0;
        }

        currentHealth = System.Math.Max(0, currentHealth - damage);

        if(damage > 0)
        {
            
            characterSheet.OnDamage();
        }

        if (currentHealth <= 0)
        {

            Die();
        }
    }

    public virtual void Heal(int healValue)
    {
        if (healValue <= 0)
        {

            return;
        }

        currentHealth = System.Math.Min(characterSheet.stats.maxHealth, currentHealth + healValue);
    }

    public void GainBarrier(int gainAmount)
    {

        if (gainAmount <= 0)
        {

            return;
        }

        currentBarrier = System.Math.Min(characterSheet.stats.maxBarrier, currentBarrier + gainAmount);    
    }

    public virtual void UpdateBarrier()
    {
        
        if(characterSheet.stats.maxBarrier == 0)
        {
            
            return;
        }

        if(currentBarrierTimer >= characterSheet.stats.barrierCooldown)
        {
            
            characterSheet.characterHealth.GainBarrier(characterSheet.stats.barrierRegen);
            currentBarrierTimer = 0;
        }
        else
        {
            
            currentBarrierTimer += 1;
        }
    }

    private void Die()
    {

        entityMgr.KillEntity(this.gameObject);
        characterSheet.OnDeath();
        Destroy(this.gameObject);
    }
}
