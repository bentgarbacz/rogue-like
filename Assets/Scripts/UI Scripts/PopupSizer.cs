using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupSizer : MonoBehaviour
{   

    void Start()
    {

        TextMeshPro toolTipText = transform.GetChild(1).GetComponent<TextMeshPro>();
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = toolTipText.GetRenderedValues() + new Vector2(10, 10);
    }
}
