using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveReferences : MonoBehaviour
{

    //this is necessary because Gameobject.find does not find inactive GameObjects. 
    //Prefabs that need references to inactive GameObjects should reference this.
    public GameObject InventoryPanel;
    public GameObject LootPanel;
}
