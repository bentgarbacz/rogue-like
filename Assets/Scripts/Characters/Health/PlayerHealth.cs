using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharacterHealth
{

    [SerializeField] private UpdateUIElements updateStats;

    public override int TakeDamage(int damage)
    {

        int damageTaken = base.TakeDamage(damage);
        updateStats.RefreshUI();

        return damageTaken;
    }

    public override void Heal(int healValue)
    {

        base.Heal(healValue);
        updateStats.RefreshUI();
    }
}
