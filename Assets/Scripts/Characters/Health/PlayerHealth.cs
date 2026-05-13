using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharacterHealth
{

    [SerializeField] private UpdateUIElements updateStats;

    public override void TakeDamage(int damage)
    {

        base.TakeDamage(damage);
        updateStats.RefreshUI();
    }

    public override void Heal(int healValue)
    {

        base.Heal(healValue);
        updateStats.RefreshUI();
    }
}
