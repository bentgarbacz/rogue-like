using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject drop;
    private DungeonManager dum;

    void Start()
    {
        
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
    }

    public void Drop()
    {

        GameObject lootBag = Instantiate(drop, transform.position, transform.rotation);
        Loot loot = lootBag.GetComponent<Loot>();
        loot.coord = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        List<Item>loots = new List<Item>();
        for(int i = 0; i < Random.Range(0, 8); i++)
        {
            loots.Add(new Potion());
        }
        
        loot.AddItems(loots);
        
        dum.AddGameObject(lootBag);
    }
}
