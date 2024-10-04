using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{

    public SpellType spellType;
    public bool targeted;
    public int minDamage = 0;
    public int maxDamage = 0;
    public int range = 0;
    public int cooldown = 0;
    public int manaCost = 0;
    public Sprite sprite;
    public ProjectileType projectileType = ProjectileType.None;
    public AudioClip castSound = null;

    public void ResetCooldown(GameObject caster)
    {

        PlayerCharacterSheet pc = caster.GetComponent<PlayerCharacterSheet>();

        if(pc.knownSpells.ContainsKey(spellType))
        {

            caster.GetComponent<PlayerCharacterSheet>().knownSpells[spellType] = 0;
        }        
    }

    public virtual bool Cast(GameObject caster, GameObject target = null)
    {

        return false;
    }
}