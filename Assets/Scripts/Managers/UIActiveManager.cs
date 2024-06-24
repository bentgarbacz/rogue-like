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
    private InventoryManager im;
    public bool mouseOverUI = false;

    void Start()
    {

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();   

        canvasHUD = GameObject.Find("CanvasHUD");

        CloseInventoryPanel();
        CloseCharacterPanel();
        CloseLootPanel();    
        HideTooltip();
        HideItemDrag();   
    }

    public bool IsPointerOverUI()
    {

        return mouseOverUI;
    }

    public void OpenInventoryPanel()
    {

        if(inventoryIsOpen == false)
        {

            inventoryIsOpen = true;
            inventoryPanel.SetActive(inventoryIsOpen);
        }
    }

    public void CloseInventoryPanel()
    {

        if(inventoryIsOpen == true)
        {

            inventoryIsOpen = false;
            inventoryPanel.SetActive(inventoryIsOpen);
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
    //container - GameObject that contains items list, used for disposal if necessary
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
    }

    public void OpenCharacterPanel()
    {

        if(characterIsOpen == false)
        {

            characterIsOpen = true;
            characterPanel.SetActive(characterIsOpen);
        }
    }

    public void CloseCharacterPanel()
    {

        if(characterIsOpen == true)
        {

            characterIsOpen = false;
            characterPanel.SetActive(characterIsOpen);
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
}
