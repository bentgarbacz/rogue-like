using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : CharacterHealth
{

    private NameplateManager npm;

    void Start()
    {
        
        npm = GameObject.Find("CanvasHUD").transform.GetChild(10).GetComponent<NameplateManager>();
    }

    public override int TakeDamage(int damage)
    {

        int damageTaken = base.TakeDamage(damage);
        npm.UpdateHealth();

        return damageTaken;
    }

    public override void Heal(int healValue)
    {

        base.Heal(healValue);
        npm.UpdateHealth();
    }
}
