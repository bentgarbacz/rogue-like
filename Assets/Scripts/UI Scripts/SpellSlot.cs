using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{

    public string spell = "";
    public Button slot;
    public Sprite defaultSprite;
    private SpellManager sm;    
    private TurnSequencer ts;
    private AudioSource audioSource;
    private AudioClip errorClip;
    [SerializeField] private SpellCaster sc;
    [SerializeField] private PlayerCharacter pc;

    void Awake()
    {
        
        GameObject managers = GameObject.Find("System Managers");
        sm = managers.GetComponent<SpellManager>();
        ts = managers.GetComponent<TurnSequencer>();
        errorClip = Resources.Load<AudioClip>("Sounds/Error");
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        slot.image.sprite = defaultSprite;
    }

    void Update()
    {

        if(spell != "")
        {

            slot.image.sprite = sm.spellDictionary[spell].sprite;
            slot.image.fillAmount = 1f - ((float)pc.knownSpells[spell] / (float)sm.spellDictionary[spell].cooldown);

        }else
        {

            slot.image.fillAmount = 1;
        }
    }

    public void SetSpell(string spell)
    {

        this.spell = spell;
    }

    public void ClearSpell()
    {

        this.spell = "";
        slot.image.sprite = defaultSprite;
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
