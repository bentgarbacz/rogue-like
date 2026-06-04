using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogEntry : MonoBehaviour
{
    public TextMeshProUGUI textEntry;
    public RectTransform rectTransform;

    public void CalculateHeightFromText()
    {

        textEntry.ForceMeshUpdate();        
        Vector2 textSize = textEntry.GetPreferredValues(textEntry.text);        
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textSize.y);
    }
}
