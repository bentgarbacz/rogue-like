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
        maxHealth = 8;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;
        evasion = 50;

        characterHealth.InitHealth(maxHealth);

        dropTable = "Skeleton";
        title = "Skull";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");

        levitating.StartLevitating();
    }
}
