using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusNotificationManager : MonoBehaviour
{

    [SerializeField] private GridLayoutGroup statusEffectGrid;
    [SerializeField] private GameObject notificationPrefab;

    //Clear status effect grid then repopulate it with up to date status effect notifications
    public void UpdateStatusNotifications(List<StatusEffect> statusEffects)
    {

        for(int i = 0; i < statusEffectGrid.transform.childCount; i++)
        {

            Destroy(statusEffectGrid.transform.GetChild(i).gameObject);
        }

        foreach(StatusEffect newStatusEffect in statusEffects)
        {

            GameObject newNotification = Instantiate(notificationPrefab);
            newNotification.GetComponent<InitiateStatusEffectNotification>().SetInfo(newStatusEffect);
            newNotification.transform.SetParent(statusEffectGrid.transform, false);
        }
    }
}
