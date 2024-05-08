using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public GameObject LootPanel;
    public Vector2Int coord;

    // Start is called before the first frame update
    void Start()
    {
        
        LootPanel = GameObject.Find("CanvasHUD").GetComponent<InactiveReferences>().LootPanel;
    }

    public void OpenContainer()
    {
        if(LootPanel){
            
            LootPanel.GetComponent<ToggleButton>().Click();
        }
    }
}
