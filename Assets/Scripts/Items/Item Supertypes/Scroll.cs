using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : Item
{  

    public int strengthRequirement = 0;
    public int dexterityRequirement = 0;
    public int intelligenceRequirement = 0;
    public SpellType spellType;
    public PlayerCharacterSheet playerCharacter;
    private readonly GameObject hero = GameObject.Find("System Managers").GetComponent<EntityManager>().hero;

    public Scroll()
    {

        sprite = Resources.Load<Sprite>("Pixel Art/Items/Scroll");
        contextClip = Resources.Load<AudioClip>("Sounds/Scroll");
        contextText = "Cast";
        playerCharacter = hero.GetComponent<PlayerCharacterSheet>();
    }


    public override void Use()
    {

        if(MeetsRequirements() && !playerCharacter.knownSpells.ContainsKey(spellType))
        {

            hero.GetComponent<TextNotificationManager>().CreateNotificationOrder(hero.transform.position, 3f, "Spell Memorized", Color.cyan, 1f);
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
