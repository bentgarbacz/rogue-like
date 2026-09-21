using UnityEngine;

public class ResoluteTechnique : CharacterModifier
{
    
    private PlayerCharacterSheet playerCharacter;
    private int critChanceHold = 0;
    private int accuracyHold = 0;

    public ResoluteTechnique(CharacterSheet affectedCharacter)
    {
        this.descriptors.Add(ModifierDescriptor.Permanent);

        this.affectedCharacter = affectedCharacter;

        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/QuestionMark");
    }

    public override void StartEffect()
    {

        critChanceHold = playerCharacter.stats.critChance;
        playerCharacter.stats.critChance = 0;
        
        accuracyHold = playerCharacter.stats.accuracy;
        playerCharacter.stats.accuracy = int.MaxValue;
    }

    public override void EndEffect()
    {
        
        playerCharacter.stats.critChance = critChanceHold;        
        playerCharacter.stats.accuracy = accuracyHold;
    }

    public override string GetDescription()
    {
        return "Your attacks cannot be evaded, but you never crit";
    }
}
