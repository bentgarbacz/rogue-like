using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public override void Start()
    {

        base.Start();
    }

    //Custom rules that describe how each enemy reacts when they see the player character
    public virtual void AggroBehavior(PlayerCharacter playerCharacter, DungeonManager dum, CombatManager cbm)
    {

        return;
    }
}
