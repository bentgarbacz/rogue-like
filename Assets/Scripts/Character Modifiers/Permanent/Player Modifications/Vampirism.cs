using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampirism : CharacterModifier
{
    
    public float lifeSteal = 1.3f;
    private PlayerCharacterSheet playerCharacter;

    public Vampirism(CharacterSheet affectedCharacter)
    {
        this.descriptors.Add(ModifierDescriptor.Permanent);
        this.descriptors.Add(ModifierDescriptor.OnHit);

        this.procChance = 1f;

        this.affectedCharacter = affectedCharacter;

        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/QuestionMark");
    }

    public override CharacterModifier GetCombatModifier(CharacterSheet targetCharacter)
    {

        return new LifeLeech(affectedCharacter, 0, lifeSteal);
    }

    public override void StartEffect()
    {
        
        playerCharacter.BecomeHungrier(playerCharacter.maxHunger);
    }

    public override string GetDescription()
    {
        return "Recover HP based on damage dealt. You are always hungry.";
    }
}