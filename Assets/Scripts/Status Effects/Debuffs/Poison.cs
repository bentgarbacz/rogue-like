using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    
    public int damageOverTime;
    private DungeonManager dum;

    public Poison(CharacterSheet affectedCharacter, int duration, int damageOverTime, DungeonManager dum)
    {

        this.type = EffectType.Debuff;
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.damageOverTime = damageOverTime;
        this.dum = dum;
    }

    public override int Effect()
    {

        affectedCharacter.TakeDamage(damageOverTime);
        affectedCharacter.GetComponent<TextNotificationManager>().CreateNotificationOrder(affectedCharacter.transform.position, 2f, damageOverTime.ToString(), Color.green);
        
        if(affectedCharacter.health <= 0)
        {

            dum.Smite(affectedCharacter.gameObject, affectedCharacter.pos);
        }

        duration -= 1;
        return duration;
    }
}
