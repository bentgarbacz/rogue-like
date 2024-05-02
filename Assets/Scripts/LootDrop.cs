using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public GameObject drop;

    private void OnDestroy()
    {
        if(!this.gameObject.scene.isLoaded)
        {

            return;
        }

        Instantiate(drop, transform.position, transform.rotation);
    }
}
