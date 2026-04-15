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
    private TurnSequencer turnSeq;
    private SpellReferences spellRef;
    private ClickManager clickMgr;
    private ToolTipManager toolTipMgr;
    private LockManager lockMgr;
    private AudioSource audioSource;
    [SerializeField] private PlayerCharacterSheet pc;
    private Mouse mouse;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        turnSeq = managers.GetComponent<TurnSequencer>();
        spellRef = managers.GetComponent<SpellReferences>();
        clickMgr = managers.GetComponent<ClickManager>();
        toolTipMgr = managers.GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();
        lockMgr = GetComponent<LockManager>();
        audioSource = GetComponent<AudioSource>();

        mouse = Mouse.current;
    }

    void LateUpdate()
    {

        if (!targeting || !mouse.leftButton.wasPressedThisFrame)
        {

            return;
        }

        GameObject target = clickMgr.GetObject();

        if(target != null && currentSpell != null)
        {

            if(currentSpell.Cast(this.gameObject, target))
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

                turnSeq.SignalAction();
            }
        }

        currentSpell = null;
        SetTargeting(false);
        toolTipMgr.DisableForceTooltip();
    }

    private void SetTargeting(bool state)
    {

        if(state)
        {

            lockMgr.AcquireTurnLock();
        }
        else
        {

            lockMgr.ReleaseTurnLock();
        }

        targeting = state;
    }

    private bool CastSpell(SpellType spellType, GameObject target = null)
    {

        Spell spell = spellRef.spellDictionary[spellType];

        if(spell.targeted && target == null)
        {

            SetTargeting(true);
            currentSpell = spell;
            toolTipMgr.EnableForceTooltip("Casting " + spellType);

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

        pc.knownSpells[spellType] = spellRef.spellDictionary[spellType].cooldown;
        pc.mana -= spellRef.spellDictionary[spellType].manaCost;
        pc.UpdateUI();
        UpdateSpellSlots();
    }

    public void SelfCast(SpellType spellType)
    {



        if(CastSpell(spellType))
        {

            SpendSpellCost(spellType);
            turnSeq.SignalAction();

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
            turnSeq.SignalAction();

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
