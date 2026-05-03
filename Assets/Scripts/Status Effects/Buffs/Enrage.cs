using UnityEngine;

public class Enrage : StatusEffect
{

    private float damageMultiplier;

    public Enrage(CharacterSheet affectedCharacter, int duration, float damageMultiplier)
    {

        this.type = EffectType.Buff;
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
        affectedCharacter.damageDealtMultiplier *= (1f + damageMultiplier);
    }

    public override void EndEffect()
    {
        affectedCharacter.damageDealtMultiplier /= (1f + damageMultiplier);
    }

    public override string GetDescription()
    {
        return "Increase damage dealt by " + (damageMultiplier * 100f).ToString("F0") + "%";
    }
}
