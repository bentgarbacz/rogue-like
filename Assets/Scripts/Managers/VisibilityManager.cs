using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{

    private Dictionary<GameObject, (ObjectLocation location, ObjectVisibility visibility)> objects = new();
    [SerializeField] private TileManager tileManager;

    public void AddObject(GameObject newObject)
    {

        if (newObject.GetComponent<ObjectVisibility>())
        {

            objects[newObject] = (newObject.GetComponent<ObjectLocation>(), newObject.GetComponent<ObjectVisibility>());
        }
    }

    public void RemoveObject(GameObject trashObject)
    {

        objects.Remove(trashObject);
    }

    public void Refresh()
    {

        objects = new();
    }

    public void UpdateVisibilities()
    {

        foreach (GameObject currentObject in objects.Keys)
        {

            ObjectVisibility vis = objects[currentObject].visibility;

            if (!vis.isActive)
            {

                continue;
            }

            Vector2Int currentCoord = objects[currentObject].location.coord;
            
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
