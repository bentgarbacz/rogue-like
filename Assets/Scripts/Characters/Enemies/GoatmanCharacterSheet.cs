using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatmanCharacterSheet : EnemyCharacterSheet
{

    bool enrageStage1 = false;
    bool enrageStage2 = false;
    bool enrageStage3 = false;

    public override void Awake()
    {
        
        base.Awake();
        stats.maxHealth = 30;
        stats.accuracy = 600;
        stats.minDamage = 1;
        stats.maxDamage = 4;
        level = 4;
        stats.speed = 8;
        stats.evasion = 50;
        

        dropTable = DropTableType.None;
        title = "Goatman";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
    }

    public override void OnDamage()
    {

        bool hasEnraged = false;

        if(characterHealth.currentHealth < stats.maxHealth * 0.6 && enrageStage1 ==false)
        {
            
            enrageStage1 = true;
            hasEnraged = true;
            characterModMgr.AddModifier(new Enrage(this, int.MaxValue, 0.3f));
        }

        if(characterHealth.currentHealth < stats.maxHealth * 0.3 && enrageStage2 ==false)
        {
            
            enrageStage2 = true;
            hasEnraged = true;
            characterModMgr.AddModifier(new Enrage(this, int.MaxValue, 0.3f));
        }

        if(characterHealth.currentHealth == 1 && enrageStage3 ==false)
        {
            
            enrageStage3 = true;
            hasEnraged = true;
            characterModMgr.AddModifier(new Enrage(this, int.MaxValue, 0.3f));
        }

        if(hasEnraged == true)
        {
            
            GetComponent<TextNotificationManager>().CreateNotificationOrder(2, "Angry", Color.red);
        }
    }

    public override void AggroBehavior()
    {

        base.AggroBehavior();
    }
}
