using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellManager : MonoBehaviour
{

    public Dictionary<string, Spell> spellDictionary = new();

    void Awake()
    {
        
        spellDictionary.Add("Heal", new Heal());
        spellDictionary.Add("Fireball", new Fireball());
        spellDictionary.Add("Teleport", new Teleport());
        spellDictionary.Add("Poisonous Strike", new PoisonousStrike());
        spellDictionary.Add("Fortify", new Fortify());
        spellDictionary.Add("Savage Leap", new SavageLeap());
        spellDictionary.Add("Slink Away", new SlinkAway());
    }
}