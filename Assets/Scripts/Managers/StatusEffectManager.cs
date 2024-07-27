using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    public List<StatusEffect> statusEffects = new();

    public void ProcessStatusEffects()
    {

        for(int i = 0; i < statusEffects.Count; i++)
        {

            if(statusEffects[i].Effect() <= 0)
            {

                statusEffects[i].EndEffect();
                statusEffects.RemoveAt(i);
            }
        }
    }
}
