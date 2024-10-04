using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : Item
{  

    public int strengthRequirement = 0;
    public int dexterityRequirement = 0;
    public int intelligenceRequirement = 0;
    public SpellType spellType;
    public PlayerCharacterSheet playerCharacter = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero.GetComponent<PlayerCharacterSheet>();

    public Scroll()
    {

        sprite = Resources.Load<Sprite>("Pixel Art/Items/Scroll");
        contextClip = Resources.Load<AudioClip>("Sounds/Scroll");
        contextText = "Cast";
    }

    public override void Use()
    {

        if(MeetsRequirements() && !playerCharacter.knownSpells.ContainsKey(spellType))
        {
            
            playerCharacter.knownSpells.Add(spellType, 0);
        }
    }

    public bool MeetsRequirements()
    {

        if(playerCharacter.strength >= strengthRequirement && playerCharacter.dexterity >= dexterityRequirement && playerCharacter.intelligence >= intelligenceRequirement)
        {

            return true;
        }

        return false;
    }
}
