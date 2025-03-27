using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameplateManager : MonoBehaviour
{

    [SerializeField]private Image bar;
    [SerializeField]private TextMeshProUGUI text;
    private EnemyCharacterSheet character;
    private ObjectHighlighter characterHighlighter;
    private UIActiveManager uiam;
    public int displayTime = 0;

    void Start()
    {
        
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }


    public void UpdateHealth()
    {
        
        if(character != null)
        {

            bar.fillAmount = (float)character.health / (float)character.maxHealth;

        }else
        {

            bar.fillAmount = 0f;
        }
    }

    public void SetCharacter(GameObject characterGameObject)
    {

        this.character = characterGameObject.GetComponent<EnemyCharacterSheet>();
        this.characterHighlighter = characterGameObject.GetComponent<ObjectHighlighter>();
        displayTime = 0;
        UpdateHealth();
        text.SetText(character.GetName());
        text.ForceMeshUpdate(true);

        uiam.OpenNameplatePanel();
    }

    public void ClearCharacter()
    {

        displayTime = 0;

        this.character = null;
        text.SetText("N/A");
        text.ForceMeshUpdate(true);

        uiam.CloseNameplatePanel();
    }

    public void IncrementDisplayTimer()
    {

        if(displayTime >= 10 && uiam.nameplatePanelIsOpen || character == null)
        {

            ClearCharacter();

        }else if(characterHighlighter.highlighted)
        {

            displayTime = 0;

        }else
        {

            displayTime += 1;
        }
    }
}
