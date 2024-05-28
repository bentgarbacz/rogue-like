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
    private DungeonManager dum;
    private InventoryManager im;

    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        canvasHUD = GameObject.Find("CanvasHUD"); 
        im = canvasHUD.GetComponent<InventoryManager>();               
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

        }else if(!inventoryIsOpen)
        {

            OpenInventoryPanel();
        }
    }

    public void OpenLootPanel(List<Item> items, GameObject container)
    {
        
        im.currentLootContainer = container;
        
        foreach(Item i in items)
        {

            foreach(ItemSlot slot in im.lootSlots)
            {

                if(slot.item == null)
                {

                    slot.AddItem(i);
                    slot.itemList = items;
                    break;
                }
            }
        }

        if(items.Count == 0)
        {

            dum.TossContainer(container);
        }
    
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

        foreach (Transform child in canvasHUD.transform)
        {

            child.gameObject.SetActive(!pauseIsOpen);
            lootPanel.SetActive(false);
            inventoryPanel.SetActive(false);
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

        }else if(!characterIsOpen)
        {

            OpenCharacterPanel();
        }
    }
}
