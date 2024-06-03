using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIActiveManager : MonoBehaviour
{

    private GameObject canvasHUD;
    public GameObject inventoryPanel;
    public bool inventoryIsOpen = false;
    public GameObject lootPanel;
    public bool lootIsOpen = false;  
    public GameObject  pausePanel;
    public bool pauseIsOpen = false;
    public GameObject characterPanel;
    public bool characterIsOpen = false; 
    public GameObject equipmentPanel;
    //public GameObject  toolTipContainer;
    //public bool toolTipContainerIsOpen = false;

    private InventoryManager im;

    void Start()
    {

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();   

        canvasHUD = GameObject.Find("CanvasHUD");            
    }

    public bool IsPointerOverUI()
    {

        return EventSystem.current.IsPointerOverGameObject();
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
    public void OpenLootPanel(List<Item> items, GameObject container)
    {
        
        im.PopulateLootSlots(items, container);
            
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

        //disable UI elements that are not  thepause menu
        foreach (Transform child in canvasHUD.transform)
        {

            child.gameObject.SetActive(!pauseIsOpen);
            lootPanel.SetActive(false);
            inventoryPanel.SetActive(false);
            characterPanel.SetActive(false);
            pausePanel.SetActive(pauseIsOpen);
        }
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

    /* public void ShowToolTip()
    {

        if(toolTipContainerIsOpen == false)
        {

            characterIsOpen = false;
            characterPanel.SetActive(characterIsOpen);
        }
    }

    public void HideToolTip()
    {

        if(toolTipContainerIsOpen == true)
        {

            characterIsOpen = false;
            characterPanel.SetActive(characterIsOpen);
        }
    } */
}
