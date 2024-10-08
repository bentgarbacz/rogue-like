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
    private Coroutine dragCoroutine;
    private const float dragDelay = 0.1f; // Delay in seconds before starting the drag

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
        
        if(mouseOver && itemSlot.item != null && Input.GetMouseButtonDown(0))
        {

            if(dragCoroutine == null)
            {

                dragCoroutine = StartCoroutine(StartDragAfterDelay());
            }

        }else if(mouseOver && Input.GetMouseButtonUp(0))
        {

            if(dragCoroutine != null)
            {

                StopCoroutine(dragCoroutine);
                dragCoroutine = null;
            }

            if(idm.itemSlot != null && itemSlot.type is not ItemSlotType.Loot)
            {

                bool isToInventory = idm.TransferToInventoryCheck(itemSlot);
                bool isValidDropOrDestroyTransfer = idm.ValidTransfertoDropOrDestroyCheck(itemSlot);
                bool isValidEquip = equm.ValidEquip(itemSlot, idm.itemSlot);

                if(isValidDropOrDestroyTransfer && isToInventory || isValidEquip)
                {
                    idm.DropItem(itemSlot);

                    if(isValidEquip)
                    {
                        equm.UpdateStats();
                    }

                    MouseExit();
                    MouseEnter();
                    return;
                }

                audioSource.PlayOneShot(errorClip);
            }
        }
    }

    private IEnumerator StartDragAfterDelay()
    {
        
        yield return new WaitForSeconds(dragDelay);

        if(mouseOver && itemSlot.item != null && Input.GetMouseButton(0))
        {

            idm.DragItem(itemSlot);
        }

        dragCoroutine = null;
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