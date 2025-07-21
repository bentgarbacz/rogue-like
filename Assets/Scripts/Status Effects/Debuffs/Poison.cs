using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    
    public int damageOverTime;
    private DungeonManager dum;
    private CharacterHealth characterHealth;

    public Poison(CharacterSheet affectedCharacter, int duration, int damageOverTime, DungeonManager dum)
    {

        this.type = EffectType.Debuff;
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.damageOverTime = damageOverTime;
        this.dum = dum;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Poison");

        characterHealth = affectedCharacter.GetComponent<CharacterHealth>();
    }

    public override int Effect()
    {

        characterHealth.TakeDamage(damageOverTime);
        affectedCharacter.GetComponent<TextNotificationManager>().CreateNotificationOrder(affectedCharacter.transform.position, 2f, damageOverTime.ToString(), Color.green);
        
        if(characterHealth.currentHealth <= 0)
        {

            dum.Smite(affectedCharacter.gameObject);
        }

        duration -= 1;
        return duration;
    }

    public override string GetDescription()
    {

        return "Take " + damageOverTime.ToString() + " damage each turn";
    }
}
