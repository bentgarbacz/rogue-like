using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{

    public string spell = "";
    public Button slot;
    public Image cooldownMask;
    public Sprite defaultSprite;
    private SpellManager sm;    
    private AudioSource audioSource;
    private AudioClip errorClip;
    [SerializeField] private SpellCaster sc;
    [SerializeField] private PlayerCharacter pc;

    void Awake()
    {
        
        GameObject managers = GameObject.Find("System Managers");
        sm = managers.GetComponent<SpellManager>();
        errorClip = Resources.Load<AudioClip>("Sounds/Error");
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        defaultSprite = Resources.Load<Sprite>("Pixel Art/UI/Inventory/emptySprite");
        slot.image.sprite = defaultSprite;
        cooldownMask.fillAmount = 0f;
    }

    void Update()
    {

        if(spell != "")
        {

            slot.image.sprite = sm.spellDictionary[spell].sprite;

            if(pc.knownSpells[spell] == 0)
            {

                cooldownMask.fillAmount = 0f;

            }else
            {

                cooldownMask.fillAmount = ((float)pc.knownSpells[spell] / (float)sm.spellDictionary[spell].cooldown);
            }

        }else
        {

            cooldownMask.fillAmount = 0f;
        }
    }

    public void SetSpell(string spell)
    {

        if(sm.spellDictionary.Keys.Contains(spell))
        {

            this.spell = spell;
            this.slot.image.sprite = sm.spellDictionary[spell].sprite;

        }else
        {

            this.spell = "";
            this.slot.image.sprite = defaultSprite;
        }
    }

    public void ClearSpell()
    {

        this.spell = "";
        this.slot.image.sprite = defaultSprite;
    }

    public void Click()
    {

        if(spell != "")
        {

            if(pc.knownSpells[spell] <= 0 && pc.mana >= sm.spellDictionary[spell].manaCost)
            {

                sc.SelfCast(spell);
                return;
            }
        }

        audioSource.PlayOneShot(errorClip);
    }
}
