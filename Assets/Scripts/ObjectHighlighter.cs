using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color startcolor;
    public Color highlightColor = Color.yellow;
    public string actionDescription = "";
    private ToolTipManager ttm;
    private NameplateManager npm;
    private UIActiveManager uiam;
    private CharacterSheet highlightedCharacter;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        highlightedCharacter = GetComponent<CharacterSheet>();
        ttm = uiam.toolTipContainer.GetComponent<ToolTipManager>();
        npm = uiam.nameplatePanel.GetComponent<NameplateManager>();
        startcolor = GetComponent<Renderer>().material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
               
        if(actionDescription != "")
        {

            ttm.SetTooltip(true, actionDescription);

        }else
        {

            uiam.HideTooltip();
        }

        if(highlightedCharacter != null)
        {
            
            if(highlightedCharacter is not PlayerCharacterSheet)
            {

                npm.SetCharacter(highlightedCharacter);

            }else
            {

                npm.ClearCharacter();
            }

        }else
        {

            npm.ClearCharacter();
        }

        GetComponent<Renderer>().material.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ttm.SetTooltip(false);
        GetComponent<Renderer>().material.color = startcolor;
    }
}
