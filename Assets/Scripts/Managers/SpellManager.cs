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
    }
}

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

public class Fireball : Spell
{
    private CombatManager cbm;

    public Fireball()
    {
        
        this.spellName = "Fireball";
        this.targeted = true;
        this.minDamage = 10;
        this.maxDamage = 20;
        this.range = 6;
        this.cooldown = 10;
        this.manaCost = 10;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Fireball");
        this.projectileType = "Fireball";
        cbm = GameObject.Find("System Managers").GetComponent<CombatManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(target.GetComponent<Character>())
        {

            float distance = Vector3.Distance(caster.transform.position, target.transform.position);

            if(LineOfSight.HasLOS(caster, target) && distance <= range)
            {

                cbm.combatBuffer.Add(new Attack(caster, target, minDamage, maxDamage, caster.GetComponent<Character>().speed, projectileType));
                ResetCooldown(caster);
                return true;
            }
        }

        return false;
    }
}

public class Teleport : Spell
{

    private DungeonManager dum;

    public Teleport()
    {
        
        this.spellName = "Teleport";
        this.targeted = true;
        this.range = 20;
        this.cooldown = 30;
        this.manaCost = 5;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Teleport");
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        float distance = Vector3.Distance(caster.transform.position, target.transform.position);

        if(distance <= range && target.GetComponent<Tile>())
        {

            if(target.GetComponent<Tile>().traversable)
            {

                Vector3 targetPos = new Vector3(target.GetComponent<Tile>().coord.x, 0, target.GetComponent<Tile>().coord.y);

                caster.GetComponent<Character>().Teleport(targetPos, dum.occupiedlist);
                ResetCooldown(caster);
                return true;
            }
        }

        return false;
    }
}

public class Heal : Spell
{

    public Heal()
    {

        this.spellName = "Heal";
        this.targeted = false;
        this.cooldown = 30;
        this.manaCost = 20;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Heal");
    }

    public override bool Cast(GameObject caster, GameObject target = null)
    {

        caster.GetComponent<Character>().Heal(10);
        ResetCooldown(caster);
        return true;
    }
}


