using UnityEngine;

public class Weaken : CharacterModifier
{

    private float damageMultiplier;

    public Weaken(CharacterSheet affectedCharacter, int duration, float damageMultiplier)
    {

        this.descriptors.Add(ModifierDescriptor.Debuff);
        this.descriptors.Add(ModifierDescriptor.Temporary);
        
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.damageMultiplier = Mathf.Clamp(damageMultiplier, 0f, 0.9999f);
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/BlueDown");
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void StartEffect()
    {
        affectedCharacter.stats.damageDealtMultiplier *= (1f - damageMultiplier);
    }

    public override void EndEffect()
    {
        affectedCharacter.stats.damageDealtMultiplier /= (1f - damageMultiplier);
    }

    public override string GetDescription()
    {
        return "Reduce damage dealt by " + (damageMultiplier * 100f).ToString("F0") + "%";
    }
}
