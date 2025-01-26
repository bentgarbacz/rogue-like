using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIActiveManager : MonoBehaviour
{

    private GameObject canvasHUD;
    public GameObject inventoryPanel;
    public bool inventoryIsOpen = true;
    public GameObject lootPanel;
    public bool lootIsOpen = true;   
    public GameObject  pausePanel;
    public bool pauseIsOpen = false;
    public GameObject characterPanel;
    public bool characterIsOpen = true; 
    public GameObject equipmentPanel;
    public GameObject infoAndControlPanel;
    public bool infoAndControlPanelIsOpen = true;
    public GameObject levelUpNotification;
    public GameObject addStrengthButton;
    public GameObject addDexterityButton;
    public GameObject addIntelligenceButton;
    public bool levelUpNotificationIsValid = false; 
    public GameObject  toolTipContainer;
    public bool toolTipContainerIsOpen = true;
    public GameObject  itemDragContainer;
    public bool itemDragContainerIsOpen = true;
    public GameObject assignSpellContainer;
    public bool assignSpellContainerIsOpen = true;
    public GameObject nameplatePanel;
    public bool nameplatePanelIsOpen = true;
    public GameObject mapPanel;
    public bool mapPanelIsOpen = true;
    private InventoryManager im;
    public bool mouseOverUI = false;

    [SerializeField]
    private GameObject dropSlot;

    [SerializeField]
    private GameObject destroySlot;

    void Start()
    {

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();   

        canvasHUD = GameObject.Find("CanvasHUD");

        CloseInventoryPanel();
        CloseCharacterPanel();
        CloseLootPanel();
        CloseNameplatePanel();
        
        pausePanel.SetActive(false);
          
        HideTooltip();
        HideItemDrag();
        HideAssignSpell(); 
        HideMap();
    }

    public bool IsPointerOverUI()
    {

        return mouseOverUI;
    }

    public void OpenInventoryPanel()
    {

        HideAssignSpell();

        if(inventoryIsOpen == false)
        {

            inventoryIsOpen = true;
            inventoryPanel.SetActive(inventoryIsOpen);
            equipmentPanel.SetActive(inventoryIsOpen);

            if(!lootIsOpen && !characterIsOpen)
            {

                dropSlot.SetActive(true);
                destroySlot.SetActive(true);
            }
        }
    }

    public void CloseInventoryPanel()
    {

        if(inventoryIsOpen == true)
        {

            inventoryIsOpen = false;
            inventoryPanel.SetActive(inventoryIsOpen);
            equipmentPanel.SetActive(inventoryIsOpen);

            dropSlot.SetActive(false);
            destroySlot.SetActive(false);

            HideItemDrag();
        }
    }

    public void ToggleInventory()
    {

        if(inventoryIsOpen)
        {

            CloseInventoryPanel();
            CloseLootPanel();

        }else if(!inventoryIsOpen && !pauseIsOpen)
        {

            CloseCharacterPanel();
            OpenInventoryPanel();
        }
    }

    //items - list of items that will be looted
    public void OpenLootPanel(List<Item> items)
    {
        
        im.PopulateLootSlots(items);
            
        if(lootIsOpen == false)
        {

            lootIsOpen = true;
            lootPanel.SetActive(lootIsOpen);
        }
    }

    public void CloseLootPanel()
    {

        if(lootIsOpen == true)
        {

            lootIsOpen = false;
            lootPanel.SetActive(lootIsOpen);

            im.ClearItemsLoot();
        }        
    }

    public void TogglePause()
    {
        
        pauseIsOpen = !pauseIsOpen;
        pausePanel.SetActive(pauseIsOpen);

        ToggleInfoAndControl();
        CloseLootPanel();
        CloseInventoryPanel();
        CloseCharacterPanel();
        HideTooltip();
        HideItemDrag();      
        HideAssignSpell();      
    }

    public void OpenCharacterPanel()
    {

        HideAssignSpell();

        if(characterIsOpen == false)
        {

            characterIsOpen = true;
            characterPanel.SetActive(characterIsOpen);
            equipmentPanel.SetActive(characterIsOpen);
        }
    }

    public void CloseCharacterPanel()
    {

        if(characterIsOpen == true)
        {

            characterIsOpen = false;
            characterPanel.SetActive(characterIsOpen);
            equipmentPanel.SetActive(characterIsOpen);
            HideItemDrag();
        }
    }

    public void ToggleCharacter()
    {

        if(characterIsOpen)
        {

            CloseCharacterPanel();

        }else if(!characterIsOpen && !pauseIsOpen)
        {

            CloseInventoryPanel();
            CloseLootPanel();
            OpenCharacterPanel();
        }
    }

    public void OpenInfoAndControlyPanel()
    {

        if(infoAndControlPanelIsOpen == false)
        {

            infoAndControlPanelIsOpen = true;
            infoAndControlPanel.SetActive(infoAndControlPanelIsOpen);
        }
    }

    public void CloseInfoAndControlPanel()
    {   

        HideAssignSpell();

        if(infoAndControlPanelIsOpen == true)
        {

            infoAndControlPanelIsOpen = false;
            infoAndControlPanel.SetActive(infoAndControlPanelIsOpen);
        }
    }

    public void ToggleInfoAndControl()
    {

        if(infoAndControlPanelIsOpen)
        {

            CloseInfoAndControlPanel();

        }else if(!infoAndControlPanelIsOpen && !pauseIsOpen)
        {

            OpenInfoAndControlyPanel();
        }
    }

    public void OpenNameplatePanel()
    {

        if(nameplatePanelIsOpen == false)
        {

            nameplatePanelIsOpen = true;
            nameplatePanel.SetActive(nameplatePanelIsOpen);
        }
    }

    public void CloseNameplatePanel()
    {   

        if(nameplatePanelIsOpen == true)
        {


            nameplatePanelIsOpen = false;
            nameplatePanel.SetActive(nameplatePanelIsOpen);
        }
    }

    public void ToggleNameplatePanel()
    {

        if(nameplatePanelIsOpen)
        {

            CloseNameplatePanel();

        }else if(!nameplatePanelIsOpen && !pauseIsOpen)
        {

            OpenNameplatePanel();
        }
    }

    public void ShowLevelUpNotifications()
    {

        if(levelUpNotificationIsValid == false)
        {

            levelUpNotificationIsValid = true;
            levelUpNotification.SetActive(levelUpNotificationIsValid);
            addStrengthButton.SetActive(levelUpNotificationIsValid);
            addDexterityButton.SetActive(levelUpNotificationIsValid);
            addIntelligenceButton.SetActive(levelUpNotificationIsValid);
        }
    }

    public void HideLevelUpNotifications()
    {

        if(levelUpNotificationIsValid == true)
        {

            levelUpNotificationIsValid = false;
            levelUpNotification.SetActive(levelUpNotificationIsValid);
            addStrengthButton.SetActive(levelUpNotificationIsValid);
            addDexterityButton.SetActive(levelUpNotificationIsValid);
            addIntelligenceButton.SetActive(levelUpNotificationIsValid);
        }
    }

    public void ShowTooltip()
    {

        if(toolTipContainerIsOpen == false)
        {

            toolTipContainerIsOpen = true;
            toolTipContainer.SetActive(toolTipContainerIsOpen);
        }
    }

    public void HideTooltip()
    {

        if(toolTipContainerIsOpen == true)
        {

            toolTipContainerIsOpen = false;
            toolTipContainer.SetActive(toolTipContainerIsOpen);
        }
    }

    public void ShowItemDrag()
    {

        if(itemDragContainerIsOpen == false)
        {

            itemDragContainerIsOpen = true;
            itemDragContainer.SetActive(itemDragContainerIsOpen);
        }
    }

    public void HideItemDrag()
    {

        if(itemDragContainerIsOpen == true)
        {

            itemDragContainerIsOpen = false;
            itemDragContainer.SetActive(itemDragContainerIsOpen);
        }
    }

    public void ShowAssignSpell()
    {

        assignSpellContainer.SetActive(false);
        assignSpellContainerIsOpen = true;
        assignSpellContainer.SetActive(assignSpellContainerIsOpen);

        assignSpellContainer.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    public void HideAssignSpell()
    {

        if(assignSpellContainerIsOpen == true)
        {

            assignSpellContainerIsOpen = false;
            assignSpellContainer.SetActive(assignSpellContainerIsOpen);
        }
    }

    public void ShowMap()
    {

        mapPanel.SetActive(false);
        mapPanelIsOpen = true;
        mapPanel.SetActive(mapPanelIsOpen);
    }

    public void HideMap()
    {

        if(mapPanelIsOpen == true)
        {

            mapPanelIsOpen = false;
            mapPanel.SetActive(mapPanelIsOpen);
        }
    }
}
