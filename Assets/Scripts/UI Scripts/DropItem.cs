using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{

    public GameObject container;
    private DungeonManager dum;
    private AudioSource audioSource;
    private AudioClip dropClip;
    private ItemSlot itemSlot;

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

        List<Item> droppedItems = new(){itemSlot.item};  

        //Determine drop location and introduce randomness to make multiple loot instances clickable on a single tile
        Vector3 dropPos = dum.hero.transform.position;
        dropPos.x += (float)(Random.Range(-20, 20) * 0.01);
        dropPos.z += (float)(Random.Range(-20, 20) * 0.01);

        GameObject lootContainer = Instantiate(container, dropPos, transform.rotation);
        Loot loot = lootContainer.GetComponent<Loot>();

        //Sets the tile coordinate in which the loot resides
        loot.coord = new Vector2Int((int)dum.hero.transform.position.x, (int)dum.hero.transform.position.z);       
        loot.AddItems(droppedItems);  

        dum.AddGameObject(lootContainer);
        dum.itemContainers.Add(loot);

        itemSlot.ThrowAway();

        GetComponentInParent<MouseOverItemSlot>().MouseExit();
        GetComponentInParent<MouseOverItemSlot>().MouseEnter();
    }
}
