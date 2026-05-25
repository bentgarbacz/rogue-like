using UnityEngine;

public class Enrage : CharacterModifier
{

    private float damageMultiplier;

    public Enrage(CharacterSheet affectedCharacter, int duration, float damageMultiplier)
    {

        this.descriptors.Add(ModifierDescriptor.Buff);
        this.descriptors.Add(ModifierDescriptor.Temporary);
        
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.damageMultiplier = Mathf.Max(damageMultiplier, 0f);
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/RedUp");
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void StartEffect()
    {
        affectedCharacter.stats.damageDealtMultiplier *= (1f + damageMultiplier);
    }

    public override void EndEffect()
    {
        affectedCharacter.stats.damageDealtMultiplier /= (1f + damageMultiplier);
    }

    public override string GetDescription()
    {
        return "Increase damage dealt by " + (damageMultiplier * 100f).ToString("F0") + "%";
    }
}
