using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameplateManager : MonoBehaviour
{

    [SerializeField]private Image healthBar;
    [SerializeField]private Image barrierBar;
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField] private GridLayoutGroup ModifierGrid;
    [SerializeField] private GameObject notificationPrefab;
    private NpcCharacterSheet characterSheet;
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

            healthBar.fillAmount = (float)characterHealth.currentHealth / (float)characterSheet.stats.maxHealth;

            if(characterSheet.stats.maxBarrier > 0)
            {

                barrierBar.fillAmount = (float)characterHealth.currentBarrier / (float)characterSheet.stats.maxBarrier;
            }
            else
            {
                
                barrierBar.fillAmount = 0f;
            }

        }else
        {

            healthBar.fillAmount = 0f;
        }
    }

    public void SetCharacter(GameObject characterGameObject)
    {

        this.characterSheet = characterGameObject.GetComponent<NpcCharacterSheet>();
        this.characterHealth = characterGameObject.GetComponent<CharacterHealth>();
        this.characterHighlighter = characterGameObject.GetComponent<ObjectHighlighter>();
        displayTime = 0;
        UpdateHealth();
        text.SetText(characterSheet.GetName());

        if (characterSheet is EnemyCharacterSheet)
        {

            healthBar.color = Color.white; // no tint for enemies, default image is red
        }
        else
        {
            
            healthBar.color = Color.green;
        }

        text.ForceMeshUpdate(true);
        UpdateModifierNotifications();
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

        UpdateModifierNotifications();
    }

    public void UpdateModifierNotifications()
    {

        if(characterSheet == null)
        {
            
            return;
        }

        for(int i = 0; i < ModifierGrid.transform.childCount; i++)
        {

            Destroy(ModifierGrid.transform.GetChild(i).gameObject);
        }

        foreach(CharacterModifier mod in characterSheet.characterModMgr.GetCharacterModifiers())
        {

            if(mod.descriptors.Contains(ModifierDescriptor.NoVisual))
            {
                
                continue;
            }

            GameObject newNotification = Instantiate(notificationPrefab);
            newNotification.GetComponent<CharacterModificationNotification>().SetInfo(mod, true);
            newNotification.transform.SetParent(ModifierGrid.transform, false);
        }
    }
}
