using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{

    public GameObject popUpPrefab;
    private HashSet<FloatingText> popups = new();
    public float speed = 2;
    public float destroyAfterDistance = 0.75f;

    void Update()
    {

        foreach(FloatingText ft in popups.ToArray())
        {

            ft.popup.transform.position = Vector3.MoveTowards(ft.popup.transform.position, ft.terminalTarget, speed * Time.deltaTime);   

            if(ft.popup.transform.position == ft.terminalTarget)
            {
                popups.Remove(ft);
                Destroy(ft.popup);
            }
        }
    }

    public void CreatePopup(Vector3 position, float height, string text, Color textColor)
    {

        GameObject popup = Instantiate(popUpPrefab, new Vector3(position.x, position.y + height, position.z), Quaternion.identity);
        TextMeshPro popUpText = popup.transform.GetChild(1).GetComponent<TextMeshPro>();
        popUpText.text = text;
        popUpText.color = textColor;



        popups.Add(new FloatingText(popup, new Vector3(popup.transform.position.x, popup.transform.position.y + destroyAfterDistance, popup.transform.position.z)));
    }

    public void CleanUp()
    {
        foreach(FloatingText ft in popups.ToArray())
        {

            Destroy(ft.popup);
        }
        
    }
}

public class FloatingText
{

    public GameObject popup;
    public Vector3 terminalTarget;

    public FloatingText(GameObject popup, Vector3 terminalTarget)
    {

        this.popup = popup;
        this.terminalTarget = terminalTarget;
    }
}
