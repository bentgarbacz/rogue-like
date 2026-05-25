using UnityEngine;

public class Resilience : CharacterModifier
{

    private float damageMultiplierTaken;

    public Resilience(CharacterSheet affectedCharacter, int duration, float damageMultiplierTaken)
    {

        this.descriptors.Add(ModifierDescriptor.Buff);
        this.descriptors.Add(ModifierDescriptor.Temporary);
        
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.damageMultiplierTaken = Mathf.Clamp(damageMultiplierTaken, 0f, 0.9999f);
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/BlueUp");
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void StartEffect()
    {
        affectedCharacter.stats.damageTakenMultiplier *= (1f - damageMultiplierTaken);
    }

    public override void EndEffect()
    {
        affectedCharacter.stats.damageTakenMultiplier /= (1f - damageMultiplierTaken);
    }

    public override string GetDescription()
    {
        return "Take " + (damageMultiplierTaken * 100f).ToString("F0") + "% less damage";
    }
}
