using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameplateManager : MonoBehaviour
{

    [SerializeField]private Image bar;
    [SerializeField]private TextMeshProUGUI text;
    private EnemyCharacterSheet characterSheet;
    private CharacterHealth characterHealth;
    private ObjectHighlighter characterHighlighter;
    private UIActiveManager uiam;
    public int displayTime = 0;

    void Start()
    {
        
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }


    public void UpdateHealth()
    {
        
        if(characterHealth != null)
        {

            bar.fillAmount = (float)characterHealth.currentHealth / (float)characterHealth.maxHealth;

        }else
        {

            bar.fillAmount = 0f;
        }
    }

    public void SetCharacter(GameObject characterGameObject)
    {

        this.characterSheet = characterGameObject.GetComponent<EnemyCharacterSheet>();
        this.characterHealth = characterGameObject.GetComponent<CharacterHealth>();
        this.characterHighlighter = characterGameObject.GetComponent<ObjectHighlighter>();
        displayTime = 0;
        UpdateHealth();
        text.SetText(characterSheet.GetName());
        text.ForceMeshUpdate(true);

        uiam.OpenNameplatePanel();
    }

    public void ClearCharacter()
    {

        displayTime = 0;

        this.characterSheet = null;
        this.characterHealth = null;
        text.SetText("N/A");
        text.ForceMeshUpdate(true);

        uiam.CloseNameplatePanel();
    }

    public void IncrementDisplayTimer()
    {

        if(displayTime >= 10 && uiam.nameplatePanelIsOpen || characterSheet == null)
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
