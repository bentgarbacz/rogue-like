using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextNotification : MonoBehaviour
{

    public GameObject notificationPrefab;
    private HashSet<FloatingText> notifications = new();
    public float speed = 2;
    public float destroyAfterDistance = 0.75f;
    public float xMin = 0.5f;
    public float xBuffer = 0.1f;
    public float yBuffer = 0.1f;

    void Update()
    {

        foreach(FloatingText ft in notifications.ToList())
        {

            ft.notification.transform.position = Vector3.MoveTowards(ft.notification.transform.position, ft.terminalTarget, speed * Time.deltaTime);   

            if(ft.notification.transform.position == ft.terminalTarget)
            {
                notifications.Remove(ft);
                Destroy(ft.notification);
            }
        }
    }

    public void CreatePopup(Vector3 position, float height, string text, Color textColor)
    {

        GameObject popup = Instantiate(notificationPrefab, new Vector3(position.x, position.y + height, position.z), Quaternion.identity);
        TextMeshPro popUpText = popup.transform.GetChild(1).GetComponent<TextMeshPro>();

        popUpText.SetText(text);
        popUpText.ForceMeshUpdate(true);

        popUpText.color = textColor;
        

        RectTransform backgroundRect = popup.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 renderedTextSize = popUpText.GetRenderedValues();
        backgroundRect.sizeDelta = new Vector2(Mathf.Max(xMin, renderedTextSize.x + xBuffer), renderedTextSize.y + yBuffer);

        
        Vector3 terminalPosition = new Vector3(popup.transform.position.x, popup.transform.position.y + destroyAfterDistance, popup.transform.position.z);

        notifications.Add(new FloatingText(popup, terminalPosition));
    }

    public void CleanUp()
    {
        foreach(FloatingText ft in notifications.ToArray())
        {

            Destroy(ft.notification);
        }
        
    }
}

public class FloatingText
{

    public GameObject notification;
    public Vector3 terminalTarget;

    public FloatingText(GameObject notification, Vector3 terminalTarget)
    {

        this.notification = notification;
        this.terminalTarget = terminalTarget;
    }
}
