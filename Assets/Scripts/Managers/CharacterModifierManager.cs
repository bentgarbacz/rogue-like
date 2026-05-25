using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterModifierManager : MonoBehaviour
{

    [SerializeField] private List<CharacterModifier> tempMods = new();
    [SerializeField] private HashSet<CharacterModifier> permMods = new();

    public void ProcessModifiers()
    {
        for (int i = tempMods.Count - 1; i >= 0; i--)
        {
            int remainingDuration = tempMods[i].Effect();

            if (remainingDuration <= 0)
            {
                tempMods[i].EndEffect();
                tempMods.RemoveAt(i);
            }
        }
    }

    public void AddModifier(CharacterModifier newMod)
    {

        if (newMod.descriptors.Contains(ModifierDescriptor.Temporary))
        {

            AddTempMod(newMod);

        }
        else if (newMod.descriptors.Contains(ModifierDescriptor.Permanent))
        {

            AddPermanentEffect(newMod);
        }

        if(newMod.descriptors.Contains(ModifierDescriptor.OneShot))
        {
            
            newMod.StartEffect();
        }
    }

    private void AddTempMod(CharacterModifier newMod)
    {
        
        PlayerCharacterSheet pc = GetComponent<PlayerCharacterSheet>();
        bool uniqueEffectFound = false;

        if (newMod.descriptors.Contains(ModifierDescriptor.Unique))
        {
            for (int i = 0; i < tempMods.Count; i++)
            {
                if (newMod.GetType() == tempMods[i].GetType())
                {
                    if (newMod.duration > tempMods[i].duration)
                    {
                        tempMods.RemoveAt(i);
                        break;
                    }

                    uniqueEffectFound = true;
                    break;
                }
            }
        }

        if (!uniqueEffectFound)
        {
            newMod.StartEffect();
            tempMods.Add(newMod);
        }

        if(pc)
        {
            
            pc.AddModificationNotification(newMod);
        }
    }

    private void AddPermanentEffect(CharacterModifier newMod)
    {
        if (permMods.Add(newMod))
        {
            newMod.StartEffect();
        }
    }

    public List<CharacterModifier> GetTempMods()
    {

        return tempMods;
    }

    public HashSet<CharacterModifier> GetPermMods()
    {

        return permMods;
    }

    public List<CharacterModifier>GetCharacterModifiers()
    {
        
        return tempMods.Concat(permMods).ToList();
    }
}
