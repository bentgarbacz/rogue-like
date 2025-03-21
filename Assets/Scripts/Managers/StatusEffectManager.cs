using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    private List<StatusEffect> statusEffects = new();

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

    public void AddEffect(StatusEffect newEffect)
    {

        PlayerCharacterSheet pc = GetComponent<PlayerCharacterSheet>();
        bool uniqueEffectFound = false;

        if(newEffect.isUnique)
        {
            
            for(int i = 0; i < statusEffects.Count; i++)
            {

                if(newEffect.type == statusEffects[i].type)
                {

                    if(newEffect.duration > statusEffects[i].duration)
                    {

                        statusEffects.RemoveAt(i);
                        break;
                    }

                    uniqueEffectFound = true;
                    break;
                }
            }
        }
        
        if(!uniqueEffectFound)
        {

            statusEffects.Add(newEffect);
        }    

        if(pc)
        {

            pc.UpdateStatusNotifications();
        }

    }

    public List<StatusEffect> GetStatusEffects()
    {

        return statusEffects;
    }
}
