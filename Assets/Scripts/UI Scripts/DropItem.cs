using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{

    public GameObject container;
    private EntityManager entityMgr;
    private TileManager tileMgr;
    private VisibilityManager visibilityManager;
    private MouseOverItemSlot mouseOverItemSlot;
    [SerializeField] private MiniMapManager miniMapManager;
    private AudioSource audioSource;
    private AudioClip dropClip;
    private ItemSlot itemSlot;
    [SerializeField] ItemDragManager idm;


    void Start()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        tileMgr = managers.GetComponent<TileManager>();
        visibilityManager = managers.GetComponent<VisibilityManager>();
        mouseOverItemSlot = GetComponentInParent<MouseOverItemSlot>();

        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        dropClip = Resources.Load<AudioClip>("Sounds/Arrow");
    }

    public void Click()
    {

        audioSource.PlayOneShot(dropClip);

        GameFunctions.DropLoot(entityMgr.hero, container, new(){itemSlot.item}, entityMgr, tileMgr, miniMapManager, visibilityManager);

        itemSlot.ThrowAway();

        mouseOverItemSlot.MouseExit();
        mouseOverItemSlot.MouseEnter();
        idm.ForgetItem();
    }
}
