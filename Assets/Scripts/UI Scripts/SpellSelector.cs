using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpellSelector : MonoBehaviour
{

    public string spell;
    private Sprite clearSprite;
    public Button spellImage;
    private SpellSlot spellSlot;
    private Tooltip spellSlotTooltip;
    private Tooltip selectorTooltip;
    private SpellReferences sm; 
    private UIActiveManager uiam;

    void Awake()
    {
        
        GameObject managers = GameObject.Find("System Managers");
        sm = managers.GetComponent<SpellReferences>();
        uiam = managers.GetComponent<UIActiveManager>();
        clearSprite = Resources.Load<Sprite>("Pixel Art/UI/X white");
        selectorTooltip = GetComponent<Tooltip>();
    }

    public void SetSpell(string spell, SpellSlot spellSlot, Tooltip tooltip)
    {

        this.spellSlot = spellSlot;
        this.spellSlotTooltip = tooltip;

        if(sm.spellDictionary.Keys.Contains(spell))
        {

            this.spell = spell;
            this.spellImage.image.sprite = sm.spellDictionary[spell].sprite;
            selectorTooltip.SetTooltip(spell);

        }else
        {

            this.spell = "";
            this.spellImage.image.sprite = clearSprite;
            selectorTooltip.SetTooltip("Clear Slot");
        }
    }

    public void Click()
    {

        spellSlot.SetSpell(spell);

        if(spell == "")
        {

            spellSlotTooltip.SetTooltip("Right click to add spell ");

        }else
        {

            spellSlotTooltip.SetTooltip("Cast " + spell + "\nMana Cost: " + sm.spellDictionary[spell].manaCost + "\nCooldown: " + sm.spellDictionary[spell].cooldown);
        }      

        uiam.HideAssignSpell();
        selectorTooltip.ClearTooltip();
    }
}
