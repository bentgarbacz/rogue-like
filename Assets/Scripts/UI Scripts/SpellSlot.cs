using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{

    public SpellType spell = SpellType.None;
    public Button slot;
    public Image cooldownMask;
    public Sprite defaultSprite;
    private SpellReferences sm;    
    private AudioSource audioSource;
    private AudioClip errorClip;
    [SerializeField] private SpellCaster sc;
    [SerializeField] private PlayerCharacterSheet pc;

    void Awake()
    {
        
        GameObject managers = GameObject.Find("System Managers");
        sm = managers.GetComponent<SpellReferences>();
        errorClip = Resources.Load<AudioClip>("Sounds/Error");
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        defaultSprite = Resources.Load<Sprite>("Pixel Art/UI/Inventory/emptySprite");
        slot.image.sprite = defaultSprite;
        cooldownMask.fillAmount = 0f;
    }

    void Update()
    {

        if(spell is not SpellType.None)
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

    public void SetSpell(SpellType spell)
    {

        if(sm.spellDictionary.Keys.Contains(spell))
        {

            this.spell = spell;
            this.slot.image.sprite = sm.spellDictionary[spell].sprite;

        }else
        {

            this.spell = SpellType.None;
            this.slot.image.sprite = defaultSprite;
        }
    }

    public void ClearSpell()
    {

        this.spell = SpellType.None;
        this.slot.image.sprite = defaultSprite;
    }

    public void Click()
    {

        if(spell is not SpellType.None)
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
