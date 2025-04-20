using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterSheet : CharacterSheet
{

    private NameplateManager npm;

    public override void Start()
    {

        base.Start();
        npm = GameObject.Find("CanvasHUD").transform.GetChild(10).GetComponent<NameplateManager>();
    }

    //Custom rules that describe how each enemy reacts when they see the player character
    public virtual void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime = 0f)
    {
        
        return;
    }

    
    public virtual bool OnAggro(DungeonManager dum, CombatManager cbm)
    {

        GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2, "!", Color.red);
        return true;
    }

    public override void Heal(int healValue)
    {

        base.Heal(healValue);
        npm.UpdateHealth();
    }

    public override int TakeDamage(int damage)
    {

        int damageTaken = base.TakeDamage(damage);
        npm.UpdateHealth();

        return damageTaken;
    }
}
