using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{

    public bool targeting = false;
    public bool selfCasting = false;
    private Spell currentSpell;
    private ItemSlot currentItemSlot;
    private Scroll currentScroll;
    private TurnSequencer ts;
    private SpellManager sm;
    private ClickManager cm;
    private ToolTipManager ttm;
    [SerializeField] private PlayerCharacter pc;
    private Mouse mouse;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        ts = managers.GetComponent<TurnSequencer>();
        sm = managers.GetComponent<SpellManager>();
        cm = managers.GetComponent<ClickManager>();
        ttm = managers.GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();

        mouse = Mouse.current;
    }

    void LateUpdate()
    {
        
        if(targeting == true)
        {

            if(mouse.leftButton.wasPressedThisFrame)
            {

                GameObject target = cm.GetObject();

                if(target != null && currentSpell != null)
                {
                    
                    bool spellCastSuccessfully = currentSpell.Cast(this.gameObject, target);

                    if(spellCastSuccessfully)
                    {

                        ts.SignalAction();

                        if(selfCasting)
                        {

                            SpendSpellCost(currentSpell.spellName);
                            selfCasting = false;
                        }

                        if(currentItemSlot != null && currentScroll != null)
                        {
                        
                            currentItemSlot.ThrowAway();
                            currentScroll.Use();
                            currentItemSlot = null;
                            currentScroll = null;
                        }
                    }
                }

                currentSpell = null;
                SetTargeting(false);
                ttm.DisableForceTooltip();
            }
        }
    }

    public void SetTargeting(bool state)
    {

        ts.gameplayHalted = state;
        targeting = state;
    }

    private bool CastSpell(string spellName, GameObject target = null)
    {

        Spell spell = sm.spellDictionary[spellName];

        if(spell.targeted && target == null)
        {

            SetTargeting(true);
            currentSpell = spell;
            ttm.EnableForceTooltip("Casting " + spellName);

            return false;
            
        }else
        {

            spell.Cast(this.gameObject);
            return true;
        }
    }

    public void SpendSpellCost(string spellName)
    {

        pc.knownSpells[spellName] = sm.spellDictionary[spellName].cooldown;
        pc.mana -= sm.spellDictionary[spellName].manaCost;
    }

    public void SelfCast(string spellName)
    {

        if(CastSpell(spellName))
        {

            SpendSpellCost(spellName);
            ts.SignalAction();

        }else{

            selfCasting = true;
        }

    }

    public void CastScroll(Scroll scroll, ItemSlot slot)
    {
       
        if(CastSpell(scroll.spellName))
        {

            scroll.Use();
            slot.ThrowAway();
            ts.SignalAction();

        }else{

            currentScroll = scroll;
            currentItemSlot = slot;
        }
    }
}
