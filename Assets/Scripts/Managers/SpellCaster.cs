using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{

    public bool targeting = false;
    public bool selfCasting = false;
    private List<SpellSlot> spellSlots = new();
    private Spell currentSpell;
    private ItemSlot currentItemSlot;
    private Scroll currentScroll;
    private TurnSequencer ts;
    private SpellReferences sm;
    private ClickManager cm;
    private ToolTipManager ttm;
    private AudioSource audioSource;
    [SerializeField] private PlayerCharacterSheet pc;
    private Mouse mouse;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        ts = managers.GetComponent<TurnSequencer>();
        sm = managers.GetComponent<SpellReferences>();
        cm = managers.GetComponent<ClickManager>();
        ttm = managers.GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();
        audioSource = GetComponent<AudioSource>();

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

                        if(currentSpell.castSound != null)
                        {

                            audioSource.PlayOneShot(currentSpell.castSound);
                        }

                        if(selfCasting)
                        {

                            SpendSpellCost(currentSpell.spellType);
                            selfCasting = false;
                        }

                        if(currentItemSlot != null && currentScroll != null)
                        {
                        
                            currentItemSlot.ThrowAway();
                            currentScroll.Use();
                            currentItemSlot = null;
                            currentScroll = null;
                        }

                        ts.SignalAction();
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

    private bool CastSpell(SpellType spellType, GameObject target = null)
    {

        Spell spell = sm.spellDictionary[spellType];

        if(spell.targeted && target == null)
        {

            SetTargeting(true);
            currentSpell = spell;
            ttm.EnableForceTooltip("Casting " + spellType);

            return false;
            
        }else
        {

            if(spell.castSound != null)
            {

                audioSource.PlayOneShot(spell.castSound);
            }

            spell.Cast(this.gameObject);
            return true;
        }
    }

    public void SpendSpellCost(SpellType spellType)
    {

        pc.knownSpells[spellType] = sm.spellDictionary[spellType].cooldown;
        pc.mana -= sm.spellDictionary[spellType].manaCost;
        UpdateSpellSlots();
    }

    public void SelfCast(SpellType spellType)
    {

        if(CastSpell(spellType))
        {

            SpendSpellCost(spellType);
            ts.SignalAction();

        }else{

            selfCasting = true;
        }

    }

    public void CastScroll(Scroll scroll, ItemSlot slot)
    {
       
        if(CastSpell(scroll.spellType))
        {

            scroll.Use();
            slot.ThrowAway();
            ts.SignalAction();

        }else{

            currentScroll = scroll;
            currentItemSlot = slot;
        }
    }

    public void RegisterSpellSlot(SpellSlot newSpellSlot)
    {

        spellSlots.Add(newSpellSlot);
    }

    public void UpdateSpellSlots()
    {

        foreach(SpellSlot currentSpellSlot in spellSlots)
        {

            currentSpellSlot.UpdateCooldown();
        }
    }
}
