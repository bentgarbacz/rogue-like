using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{

    public string spellName;
    public bool targeted;
    public int minDamage = 0;
    public int maxDamage = 0;
    public int range = 0;
    public int cooldown = 0;
    public int manaCost = 0;
    public Sprite sprite;
    public string projectileType = "None";
    public AudioClip castSound = null;

    public void ResetCooldown(GameObject caster)
    {

        PlayerCharacter pc = caster.GetComponent<PlayerCharacter>();

        if(pc.knownSpells.ContainsKey(spellName))
        {

            caster.GetComponent<PlayerCharacter>().knownSpells[spellName] = 0;
        }        
    }

    public virtual bool Cast(GameObject caster, GameObject target = null)
    {

        return false;
    }
}