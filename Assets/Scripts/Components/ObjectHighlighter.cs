using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.yellow;
    public string actionDescription = "";
    private ToolTipManager ttm;
    private NameplateManager npm;
    private UIActiveManager uiam;
    private CharacterSheet highlightedCharacter;
    private Renderer gameObjectRenderer;
    public bool highlighted = false;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        highlightedCharacter = GetComponent<CharacterSheet>();
        ttm = uiam.toolTipContainer.GetComponent<ToolTipManager>();
        npm = uiam.nameplatePanel.GetComponent<NameplateManager>();
        gameObjectRenderer = GetComponent<Renderer>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        EnterBehavior();
    }

    private void EnterBehavior()
    {

        if (actionDescription != "")
        {

            ttm.SetTooltip(true, actionDescription);
        }

        if (highlightedCharacter != null)
        {

            if (highlightedCharacter is NpcCharacterSheet)
            {

                npm.SetCharacter(this.gameObject);

            }
        }

        gameObjectRenderer.material.color = ColorWithAlphaPreserved(highlightColor);
        highlighted = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ttm.SetTooltip(false);

        gameObjectRenderer.material.color = ColorWithAlphaPreserved(defaultColor);
        highlighted = false;
    }


    public void OnDestroy()
    {

        if (highlighted == true)
        {

            ClearTooltip();
        }
    }

    public void OnDisable()
    {

        if (highlighted == true)
        {

            ClearTooltip();
        }
    }

    public void ClearTooltip()
    {

        ttm.SetTooltip(false);
    }

    public void SetActionText(string text)
    {

        actionDescription = text;

        if (highlighted)
        {

            EnterBehavior();
        }
    }

    private Color ColorWithAlphaPreserved(Color color)
    {

        Color modifiedColor = color;
        modifiedColor.a = gameObjectRenderer.material.color.a;
        return modifiedColor;
    }
}
