
using UnityEngine;

public class Heal : Spell
{

    private int healValue = 10;
    
    public Heal()
    {

        this.spellType = SpellType.Heal;
        this.targeted = false;
        this.cooldown = 30;
        this.manaCost = 20;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Heal");
        this.castSound = Resources.Load<AudioClip>("Sounds/Heal");
    }

    public override bool Cast(GameObject caster, GameObject target = null)
    {

        caster.GetComponent<CharacterHealth>().Heal(healValue);
        ResetCooldown(caster);
        return true;
    }
}
