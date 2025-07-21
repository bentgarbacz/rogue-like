using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellReferences : MonoBehaviour
{

    public Dictionary<SpellType, Spell> spellDictionary = new();

    void Awake()
    {

        spellDictionary.Add(SpellType.Heal, new Heal());
        spellDictionary.Add(SpellType.Fireball, new Fireball());
        spellDictionary.Add(SpellType.Teleport, new Teleport());
        spellDictionary.Add(SpellType.PoisonousStrike, new PoisonousStrike());
        spellDictionary.Add(SpellType.Fortify, new Fortify());
        spellDictionary.Add(SpellType.SavageLeap, new SavageLeap());
        spellDictionary.Add(SpellType.SlinkAway, new SlinkAway());
        spellDictionary.Add(SpellType.Clairvoyance, new Clairvoyance());
        spellDictionary.Add(SpellType.Telekinesis, new Telekinesis());
        spellDictionary.Add(SpellType.SummonSkull, new SummonSkull());
    }
}