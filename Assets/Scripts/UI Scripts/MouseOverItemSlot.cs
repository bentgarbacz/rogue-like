using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> children = new();
    public bool state = false;
    public bool mouseOver = false;
    private ItemSlot itemSlot;
    public AudioSource audioSource;
    public AudioClip errorClip;
    private ToolTipManager ttm;
    private ItemDragManager idm;
    private EquipmentManager equm;
    //private Coroutine dragCoroutine;

    void Awake()
    {

        itemSlot = GetComponent<ItemSlot>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        errorClip = Resources.Load<AudioClip>("Sounds/Error");
        ttm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();
        idm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().itemDragContainer.GetComponent<ItemDragManager>();
        equm = GameObject.Find("System Managers").GetComponent<EquipmentManager>();

        foreach(Transform child in transform)
        {

            children.Add(child.gameObject);
        }

        SetChildren(state);
    }

    void Update()
    {
        
        if(mouseOver && Input.GetMouseButtonUp(0))
        {

            if(!idm.isItemHeld && itemSlot.item != null)
            {

                idm.DragItem(itemSlot);

            }else if(idm.isItemHeld)
            {

                if(idm.itemSlot != null && itemSlot.type is not ItemSlotType.Loot)
                {

                    bool isToInventory = idm.TransferToInventoryCheck(itemSlot);
                    bool isValidDropOrDestroyTransfer = idm.ValidTransferToDropOrDestroyCheck(itemSlot);
                    bool isValidEquip = equm.ValidEquip(itemSlot, idm.itemSlot);
                    bool isValidEquipInverse = equm.ValidEquip(idm.itemSlot, itemSlot);

                    if(!idm.IsInverseEquip(itemSlot))
                    {

                        isValidEquipInverse = true;
                    }

                    if(isValidDropOrDestroyTransfer && isToInventory && isValidEquipInverse|| isValidEquip)
                    {
                       
                       idm.DropItem(itemSlot);

                        if(isValidEquip)
                        {
                            equm.UpdateStats();
                        }

                        MouseExit();
                        MouseEnter();
                        idm.ForgetItem();
                        return;
                    }

                    idm.ForgetItem();
                    audioSource.PlayOneShot(errorClip);
                }
            }
        }
    }

    private void SetChildren(bool newState)
    {

        idm.paused = false;

        foreach(GameObject child in children)
        {

            child.SetActive(newState);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        mouseOver = true;
        MouseEnter();
    }

    public void MouseEnter()
    {

        if(state == false && itemSlot.item != null)
        {

            state = !state;
            SetChildren(state);

            ttm.SetTooltip(true, itemSlot.GetSummary());

            if(transform.GetChild(0).GetComponent<InventoryContext>())
            {

                transform.GetChild(0).GetComponent<InventoryContext>().SetText();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        mouseOver = false;
        ttm.SetTooltip(false);
        MouseExit();
    }

    public void MouseExit()
    {

        if(state == true)
        {

            state = !state;
            SetChildren(state);
        }
    }

    public void OnDisable()
    {

        mouseOver = false;
        ttm.SetTooltip(false);
        MouseExit();
    }
}