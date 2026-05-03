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
        maxHealth = 30;
        accuracy = 600;
        minDamage = 1;
        maxDamage = 4;
        level = 4;
        speed = 8;
        evasion = 50;
        

        characterHealth.InitHealth(maxHealth);

        dropTable = DropTableType.None;
        title = "Goatman";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
    }

    public override void OnDamage()
    {

        bool hasEnraged = false;

        if(characterHealth.currentHealth < characterHealth.maxHealth * 0.6 && enrageStage1 ==false)
        {
            
            enrageStage1 = true;
            hasEnraged = true;
            statusEffectMgr.AddEffect(new Enrage(this, int.MaxValue, 0.3f));
        }

        if(characterHealth.currentHealth < characterHealth.maxHealth * 0.3 && enrageStage2 ==false)
        {
            
            enrageStage2 = true;
            hasEnraged = true;
            statusEffectMgr.AddEffect(new Enrage(this, int.MaxValue, 0.3f));
        }

        if(characterHealth.currentHealth == 1 && enrageStage3 ==false)
        {
            
            enrageStage3 = true;
            hasEnraged = true;
            statusEffectMgr.AddEffect(new Enrage(this, int.MaxValue, 0.3f));
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
