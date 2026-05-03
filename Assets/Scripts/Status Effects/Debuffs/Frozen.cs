using UnityEngine;

public class Frozen : StatusEffect
{

    private Renderer renderer;
    private ObjectHighlighter objectHighlighter;

    public Frozen(CharacterSheet affectedCharacter, int duration)
    {
        this.type = EffectType.Debuff;
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Frozen");
        this.renderer = affectedCharacter.GetComponent<Renderer>();
        this.objectHighlighter = affectedCharacter.GetComponent<ObjectHighlighter>();
    }

    public override int Effect()
    {
        duration -= 1;
        return duration;
    }

    public override void StartEffect()
    {
        affectedCharacter.isActionBlocked = true;
        
        objectHighlighter.defaultColor = Color.cyan;
        renderer.material.color = objectHighlighter.defaultColor;
        
        affectedCharacter.GetComponent<TextNotificationManager>()?.CreateNotificationOrder(3f, "Frozen", Color.cyan, 1f);
    }

    public override void EndEffect()
    {
        affectedCharacter.isActionBlocked = false;
        renderer.material.color = Color.white;
        objectHighlighter.defaultColor = Color.white;
    }

    public override string GetDescription()
    {
        return "Cannot perform actions for " + duration.ToString() + " turns.";
    }
}
