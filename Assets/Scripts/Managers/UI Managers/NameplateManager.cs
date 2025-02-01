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
    private UIActiveManager uiam;

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

    public void SetCharacter(EnemyCharacterSheet character)
    {

        this.character = character;
        character.focused = true;
        UpdateHealth();
        text.SetText(character.GetName());
        text.ForceMeshUpdate(true);

        uiam.OpenNameplatePanel();
    }

    public void ClearCharacter()
    {

        if(this.character != null)
        {

            this.character.focused = false;
        }

        this.character = null;
        text.SetText("N/A");
        text.ForceMeshUpdate(true);

        uiam.CloseNameplatePanel();
    }
}
