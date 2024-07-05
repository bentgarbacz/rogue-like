using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{

    public Dictionary<string, Spell> spellDictionary = new();

    void Awake()
    {
        spellDictionary.Add("Heal", new Heal());
        spellDictionary.Add("Fireball", new Fireball());
    }
}

public class Spell
{

    public bool targeted;
    public int minDamage = 0;
    public int maxDamage = 0;
    public int range = 0;
    public string projectileType = "None";

    public virtual void Cast(GameObject caster, GameObject target = null){}
}

public class Fireball : Spell
{
    private CombatManager cbm;

    public Fireball()
    {
                
        this.targeted = true;
        this.minDamage = 10;
        this.maxDamage = 20;
        this.range = 6;
        this.projectileType = "Fireball";
        cbm = GameObject.Find("System Managers").GetComponent<CombatManager>();
    }

    public override void Cast(GameObject caster, GameObject target)
    {

        float distance = Vector3.Distance(caster.transform.position, target.transform.position);

        if(LineOfSight.HasLOS(caster, target) && distance <= range)
        {

            cbm.combatBuffer.Add(new Attack(caster, target, minDamage, maxDamage, caster.GetComponent<Character>().speed, projectileType));
        }
    }
}

public class Heal : Spell
{

    public Heal()
    {

        this.targeted = false;
    }

    public override void Cast(GameObject caster, GameObject target = null)
    {

        caster.GetComponent<Character>().Heal(10);
    }
}


