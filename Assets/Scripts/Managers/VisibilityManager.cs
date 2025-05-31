using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{

    public HashSet<GameObject> objects = new();
    [SerializeField] private TileManager tileManager;

    public void AddObject(GameObject newObject)
    {

        if (newObject.GetComponent<ObjectVisibility>())
        {

            objects.Add(newObject);
        }
    }

    public void Refresh()
    {

        objects = new();
    }

    public void UpdateVisibilities()
    {

        foreach (GameObject currentObject in objects)
        {

            ObjectVisibility vis = currentObject.GetComponent<ObjectVisibility>();

            if (!vis.isActive)
            {

                continue;
            }

            EnemyCharacterSheet npc = currentObject.GetComponent<EnemyCharacterSheet>();
            Interactable interactable = currentObject.GetComponent<Interactable>();

            Vector2Int currentCoord;

            if (npc != null)
            {

                currentCoord = npc.coord;
            }
            else if (interactable != null)
            {

                currentCoord = interactable.coord;

            }
            else
            {

                continue;
            }
            
            if (tileManager.revealedTiles.Contains(currentCoord))
            {

                vis.SetVisibility(true);

            } else
            {

                vis.SetVisibility(false);
            }
        }
    }
}
