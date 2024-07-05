using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{

    public bool targeting = false;
    private Spell currentSpell;
    private TurnSequencer ts;
    private SpellManager sm;
    private ClickManager cm;
    private Mouse mouse;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        ts = managers.GetComponent<TurnSequencer>();
        sm = managers.GetComponent<SpellManager>();
        cm = managers.GetComponent<ClickManager>();

        mouse = Mouse.current;
    }

    void Update()
    {
        
        if(targeting == true)
        {

            if(mouse.leftButton.wasPressedThisFrame)
            {

                GameObject target = cm.GetObject();

                if(target.GetComponent<Character>() && currentSpell != null)
                {
                    
                    currentSpell.Cast(this.gameObject, target);
                }

                currentSpell = null;
                EnableTargeting(false);
            }
        }
    }

    public void EnableTargeting(bool state)
    {

        ts.gameplayHalted = state;
        targeting = state;
    }

    public bool CastSpell(string spellName)
    {
        Spell spell = sm.spellDictionary[spellName];

        if(spell.targeted)
        {

            EnableTargeting(true);
            currentSpell = spell;

            return true;
            
        }else
        {

            spell.Cast(this.gameObject);
            return true;
        }

        return false;
    }

    public void CastScroll(Scroll scroll, ItemSlot slot)
    {
       
        if(CastSpell(scroll.spellName))
        {

            scroll.Use();
            slot.ThrowAway();
            ts.SignalAction();

        }
    }
}
