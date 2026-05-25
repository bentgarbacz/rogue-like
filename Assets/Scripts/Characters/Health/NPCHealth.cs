using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : CharacterHealth
{

    private NameplateManager npm;

    public override void Awake()
    {
        base.Awake();
        npm = GameObject.Find("CanvasHUD").transform.GetChild(10).GetComponent<NameplateManager>();        
    }

    public override void TakeDamage(int damage)
    {

        base.TakeDamage(damage);
        npm.UpdateHealth();
    }

    public override void Heal(int healValue)
    {

        base.Heal(healValue);
        npm.UpdateHealth();
    }
}
