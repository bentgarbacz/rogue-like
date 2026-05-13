using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Levitating))]
public class SkullCharacterSheet : MinionCharacterSheet
{

    [SerializeField] private Levitating levitating;

    public override void Awake()
    {

        base.Awake();
        stats.maxHealth = 8;
        stats.accuracy = 100;
        stats.minDamage = 1;
        stats.maxDamage = 3;
        level = 3;
        stats.speed = 11;
        stats.evasion = 50;

        dropTable = DropTableType.Skeleton;
        title = "Skull";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");

        levitating.StartLevitating();
    }
}
