using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeys : MonoBehaviour
{

    private GameObject canvasHUD;
    private InventoryManager im;
    private InactiveReferences ir;
    private bool stateHUD = true;

    void Start()
    {

        canvasHUD = GameObject.Find("CanvasHUD");
        im = canvasHUD.GetComponent<InventoryManager>();
        ir = canvasHUD.GetComponent<InactiveReferences>();
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            TogglePause();
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            
            im.ToggleInventory();
        }
    }

    public void TogglePause()
    {
        stateHUD = !stateHUD;

            foreach (Transform child in canvasHUD.transform)
            {

                child.gameObject.SetActive(stateHUD);
                ir.LootPanel.SetActive(false);
                ir.InventoryPanel.SetActive(false);
                ir.PausePanel.SetActive(!stateHUD);
            }
    }
    
}
