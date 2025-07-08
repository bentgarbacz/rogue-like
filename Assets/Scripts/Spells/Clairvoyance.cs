
using UnityEngine;

public class Clairvoyance : Spell
{
    private VisibilityManager vm;
    private TileManager tm;

    public Clairvoyance()
    {

        this.spellType = SpellType.Clairvoyance;
        this.targeted = false;
        this.cooldown = 50;
        this.manaCost = 30;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/eye");
        this.castSound = Resources.Load<AudioClip>("Sounds/confuse");

        GameObject managers = GameObject.Find("System Managers");

        vm = managers.GetComponent<VisibilityManager>();
        tm = managers.GetComponent<TileManager>();
    }

    public override bool Cast(GameObject caster, GameObject target = null)
    {

        tm.RevealAllTiles();
        vm.ForceAllVisibility(true);
        ResetCooldown(caster);
        return true;
    }
}
