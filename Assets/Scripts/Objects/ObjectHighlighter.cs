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
    public bool highlighted = false;

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
        }

        if(highlightedCharacter != null)
        {
            
            if(highlightedCharacter is EnemyCharacterSheet)
            {

                npm.SetCharacter(this.gameObject);

            }
        }

        GetComponent<Renderer>().material.color = highlightColor;
        highlighted = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ttm.SetTooltip(false);
        GetComponent<Renderer>().material.color = startcolor;
        highlighted = false;
    }


    public void OnDestroy()
    {

        if(highlighted == true)
        {
            Debug.Log("Destroy");
            Debug.Log(actionDescription);

            ClearTooltip();   
        }
    }

    public void OnDisable()
    {

        if(highlighted == true)
        {

            Debug.Log("disable");
            Debug.Log(actionDescription);
            ClearTooltip();   
        }
    }

    public void ClearTooltip()
    {

        ttm.SetTooltip(false);
    }
}
