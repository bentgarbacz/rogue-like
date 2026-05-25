using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : CharacterModifier
{
    
    private EntityManager entityMgr;
    private Renderer renderer;

    public Stealth(CharacterSheet affectedCharacter, int duration, EntityManager entityMgr)
    {

        this.descriptors.Add(ModifierDescriptor.Buff);
        this.descriptors.Add(ModifierDescriptor.Temporary);
        this.descriptors.Add(ModifierDescriptor.Unique);

        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.entityMgr = entityMgr;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Slink Away");

        renderer = affectedCharacter.GetComponent<Renderer>();
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void StartEffect()
    {
        
        TransparencyManager.SetTransparency(renderer, 0.3f);
        entityMgr.enemiesOnLookout = false;
        entityMgr.ClearAggroBuffer();
    }

    public override void EndEffect()
    {
        
        TransparencyManager.SetOpaque(renderer);
        entityMgr.enemiesOnLookout = true;
    }

    public override string GetDescription()
    {

        return "Unseen by enemies";
    }
}
