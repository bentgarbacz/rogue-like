using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject drop;

    private void OnDestroy()
    {
        if(!this.gameObject.scene.isLoaded)
        {

            return;
        }

        GameObject lootBag = Instantiate(drop, transform.position, transform.rotation);
        lootBag.GetComponent<Loot>().coord = new Vector2Int((int)transform.position.x, (int)transform.position.z);
    }
}
