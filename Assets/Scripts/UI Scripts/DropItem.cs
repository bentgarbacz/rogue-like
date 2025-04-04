using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{

    public GameObject container;
    private DungeonManager dum;
    [SerializeField] private MiniMapManager miniMapManager;
    private AudioSource audioSource;
    private AudioClip dropClip;
    private ItemSlot itemSlot;
    [SerializeField] ItemDragManager idm;


    void Start()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        dropClip = Resources.Load<AudioClip>("Sounds/Arrow");
    }

    public void Click()
    {

        audioSource.PlayOneShot(dropClip);

        GameFunctions.DropLoot(dum.hero, container, new(){itemSlot.item}, dum, miniMapManager);

        itemSlot.ThrowAway();

        GetComponentInParent<MouseOverItemSlot>().MouseExit();
        GetComponentInParent<MouseOverItemSlot>().MouseEnter();
        idm.ForgetItem();
    }
}
