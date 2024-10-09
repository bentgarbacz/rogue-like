using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameplateManager : MonoBehaviour
{

    [SerializeField]private Image bar;
    [SerializeField]private TextMeshProUGUI text;
    private CharacterSheet character;
    private UIActiveManager uiam;

    void Start()
    {
        
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }


    void Update()
    {
        
        if(character != null)
        {

            bar.fillAmount = (float)character.health / (float)character.maxHealth;
        }
    }

    public void SetCharacter(CharacterSheet character)
    {

        this.character = character;
        text.SetText(character.GetName());
        text.ForceMeshUpdate(true);

        uiam.OpenNameplatePanel();
    }

    public void ClearCharacter()
    {

        this.character = null;
        text.SetText("N/A");
        text.ForceMeshUpdate(true);

        uiam.CloseNameplatePanel();
    }
}
