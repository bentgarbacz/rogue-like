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

        if(notificationCreationCleared == false)
        {

            return;
        }

        if(notificationOrders.Count > 0)
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

    private void CreateNotification(Vector3 position, float spawnOffsetY, string text, Color textColor, float speed = 2f)
    {

        GameObject newNotification = Instantiate(notificationPrefab, new Vector3(position.x, position.y + spawnOffsetY, position.z), Quaternion.identity);
        TextNotification textNotification = newNotification.GetComponent<TextNotification>();
        
        textNotification.text.SetText(text);
        textNotification.text.color = textColor;
        textNotification.text.ForceMeshUpdate(true);           
       
        Vector2 renderedTextSize = textNotification.text.GetRenderedValues();
        textNotification.background.rectTransform.sizeDelta = new Vector2(Mathf.Max(xMin, renderedTextSize.x + xBuffer), renderedTextSize.y + yBuffer);
        //textNotification.text.transform.SetAsLastSibling();
        
        Vector3 terminalPosition = new(newNotification.transform.position.x, 
                                       newNotification.transform.position.y + destroyAfterDistance, 
                                       newNotification.transform.position.z);

        textNotification.InitText(terminalPosition, speed);
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


