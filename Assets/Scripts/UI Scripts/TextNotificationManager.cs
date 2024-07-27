using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextNotificationManager : MonoBehaviour
{

    public GameObject notificationPrefab;
    public float destroyAfterDistance = 0.75f;
    public float xMin = 0.5f;
    public float xBuffer = 0.1f;
    public float yBuffer = 0.1f;
    public List<NotificationOrder> notificationOrders = new();
    private float waitTime = 0.33f;
    private bool notificationCreationCleared = true;

    void Update()
    {

        if(notificationCreationCleared == true && notificationOrders.Count > 0)
        {

            notificationCreationCleared = false;

            NotificationOrder currentNotification = notificationOrders[0];
            notificationOrders.RemoveAt(0);

            CreateNotification(currentNotification.position, currentNotification.height, currentNotification.text, currentNotification.textColor, currentNotification.speed);
            StartCoroutine(WaitForNotification());
        }
    }

    public void CreateNotificationOrder(Vector3 position, float height, string text, Color textColor, float speed = 2f)
    {

        notificationOrders.Add(new NotificationOrder(position, height, text, textColor, speed));
    }

    private IEnumerator WaitForNotification()
    {

        yield return new WaitForSeconds(waitTime);
        notificationCreationCleared = true;
    }   

    private void CreateNotification(Vector3 position, float height, string text, Color textColor, float speed = 2f)
    {

        GameObject notification = Instantiate(notificationPrefab, new Vector3(position.x, position.y + height, position.z), Quaternion.identity);
        TextMeshPro notificationText = notification.transform.GetChild(1).GetComponent<TextMeshPro>();

        notificationText.SetText(text);
        notificationText.ForceMeshUpdate(true);

        notificationText.color = textColor;        

        RectTransform backgroundRect = notification.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 renderedTextSize = notificationText.GetRenderedValues();
        backgroundRect.sizeDelta = new Vector2(Mathf.Max(xMin, renderedTextSize.x + xBuffer), renderedTextSize.y + yBuffer);
        
        Vector3 terminalPosition = new Vector3(notification.transform.position.x, notification.transform.position.y + destroyAfterDistance, notification.transform.position.z);

        notification.GetComponent<TextNotification>().InitText(terminalPosition, speed);
    }    
}

public class NotificationOrder
{

    public Vector3 position;
    public float height;
    public string text;
    public Color textColor;
    public float speed;

    public NotificationOrder(Vector3 position, float height, string text, Color textColor, float speed)
    {

        this.position = position;
        this.height = height;
        this.text = text;
        this.textColor = textColor;
        this.speed = speed;
    }
}


