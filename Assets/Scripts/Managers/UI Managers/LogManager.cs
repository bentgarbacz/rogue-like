using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField] private GameObject logEntry;
    [SerializeField] private VerticalLayoutGroup verticalLayout;
    [SerializeField] private RectTransform verticalLayoutTransform;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private IsMouseOverUI mouseOver;
    private int maxLogEntries = 500;
    private const float ENTRY_SPACING = 15f;

    public GameObject CreateLogEntry(string message, Color color)
    {
        
        TrimOldEntries();
        
        GameObject entryInstance = Instantiate(logEntry, verticalLayout.transform);
        entryInstance.SetActive(true);

        LogEntry log = entryInstance.GetComponent<LogEntry>();
        log.textEntry.text = message;
        log.textEntry.color = color;
        log.CalculateHeightFromText();

        UpdateLayoutHeight();
        scrollRect.verticalNormalizedPosition = 0f;

        return entryInstance;
    }

    private void TrimOldEntries()
    {

        int childCount = verticalLayout.transform.childCount;

        while (childCount >= maxLogEntries)
        {

            DestroyImmediate(verticalLayout.transform.GetChild(0).gameObject);
            childCount = verticalLayout.transform.childCount;
        }

        UpdateLayoutHeight();
    }

    public void ClearLog()
    {

        for (int i = verticalLayout.transform.childCount - 1; i >= 0; i--)
        {

            DestroyImmediate(verticalLayout.transform.GetChild(i).gameObject);
        }

        UpdateLayoutHeight();
    }

    private void UpdateLayoutHeight()
    {

        int entryCount = verticalLayout.transform.childCount;
        float totalHeight = 0f;

        for (int i = 0; i < entryCount; i++)
        {
            LayoutElement layoutElement = verticalLayout.transform.GetChild(i).GetComponent<LayoutElement>();
            if (layoutElement != null)
            {

                totalHeight += layoutElement.preferredHeight;
            }
            
            if (i < entryCount - 1)
            {

                totalHeight += ENTRY_SPACING;
            }
        }
    
        verticalLayoutTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }

    public bool IsMouseOver()
    {
        
        return mouseOver.IsMouseOverSelf();
    }
}
