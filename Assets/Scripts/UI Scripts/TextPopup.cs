using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{

    public GameObject popUpPrefab;

    public void CreatePopup(Vector3 position, float height, string text, Color textColor)
    {

        var popup = Instantiate(popUpPrefab, new Vector3(position.x, position.y + height, position.z), Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshPro>();
        temp.text = text;
        temp.color = textColor;

        Destroy(popup, 1f);
    }
}
