using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Vector2Int coord;
    public List<Item> items;
    private UIActiveManager uiam;
    private AudioSource audioSource;

    void Start()
    {
        
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void AddItems(List<Item> newItems)
    {

        items = newItems;
    }

    public void OpenContainer(GameObject container)
    {

        audioSource.Play();

        uiam.OpenLootPanel(items, container);        
        uiam.OpenInventoryPanel();
    }
}
