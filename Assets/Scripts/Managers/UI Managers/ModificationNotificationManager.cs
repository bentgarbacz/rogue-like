using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModificationNotificationManager : MonoBehaviour
{

    [SerializeField] private GridLayoutGroup ModifierGrid;
    [SerializeField] private GameObject notificationPrefab;
    private Dictionary<CharacterModifier, CharacterModificationNotification> notificationDict = new();

    //Clear modification grid then repopulate it with up to date modification notifications
    public void UpdateModifierNotifications()
    {

        List<CharacterModifier> trashList = new();

        foreach(CharacterModifier currMod in notificationDict.Keys)
        {

            notificationDict[currMod].SetDuration(currMod.duration.ToString());

            if(currMod.duration <= 0)
            {
                
                trashList.Add(currMod);
                Destroy(notificationDict[currMod].gameObject);
            }
        }

        foreach(CharacterModifier trashMod in trashList)
        {
            
            notificationDict.Remove(trashMod);
        }
    }

    public void AddMod(CharacterModifier newMod)
    {
        if(newMod.descriptors.Contains(ModifierDescriptor.NoVisual))
        {
            
            return;
        }

        GameObject newNotification = Instantiate(notificationPrefab);
        CharacterModificationNotification charModNotification = newNotification.GetComponent<CharacterModificationNotification>();
        charModNotification.SetInfo(newMod);
        newNotification.transform.SetParent(ModifierGrid.transform, false);

        notificationDict.Add(newMod, charModNotification);
    }
}
