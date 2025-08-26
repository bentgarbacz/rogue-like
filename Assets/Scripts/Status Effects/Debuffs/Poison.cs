using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    
    public int damageOverTime;
    private CharacterHealth characterHealth;

    public Poison(CharacterSheet affectedCharacter, int duration, int damageOverTime, EntityManager entityMgr)
    {

        this.type = EffectType.Debuff;
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.damageOverTime = damageOverTime;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Poison");

        characterHealth = affectedCharacter.GetComponent<CharacterHealth>();
    }

    public override int Effect()
    {

        TextNotificationManager textNotificationMgr = affectedCharacter.GetComponent<TextNotificationManager>();
        textNotificationMgr.CreateNotificationOrder(affectedCharacter.transform.position, 2f, damageOverTime.ToString(), Color.green);
        characterHealth.TakeDamage(damageOverTime);

        duration -= 1;
        return duration;
    }

    public override string GetDescription()
    {

        return "Take " + damageOverTime.ToString() + " damage each turn";
    }
}
