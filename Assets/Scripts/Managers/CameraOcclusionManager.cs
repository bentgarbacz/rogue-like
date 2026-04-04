using System.Collections.Generic;
using UnityEngine;

public class CameraOcclusionManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    // Tracks walls we have forced invisible so we can restore them when they are no longer between camera and player.
    private readonly HashSet<GameObject> hiddenWalls = new();

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;
        Ray ray = new Ray(transform.position, direction.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        // Determine which walls are currently between the camera and the player.
        var currentWalls = new HashSet<GameObject>();
        foreach (RaycastHit hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            if (IsWall(obj))
            {
                currentWalls.Add(obj);
                if (!hiddenWalls.Contains(obj))
                {
                    SetWallAlpha(obj, 0f);
                    hiddenWalls.Add(obj);
                }
            }
        }

        // Restore any walls that are no longer between the camera and player.
        var toRestore = new List<GameObject>();
        foreach (GameObject wall in hiddenWalls)
        {
            if (!currentWalls.Contains(wall))
            {
                SetWallAlpha(wall, 1f);
                toRestore.Add(wall);
            }
        }

        foreach (GameObject wall in toRestore)
        {
            hiddenWalls.Remove(wall);
        }
    }

    private bool IsWall(GameObject obj)
    {
        Tile tile = obj.GetComponent<Tile>();
        return tile != null && tile.GetIconType() == IconType.Wall;
    }

    private void SetWallAlpha(GameObject wall, float alpha)
    {
        MeshRenderer mr = wall.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mr.GetPropertyBlock(mpb);
            Color color = mr.material.color;
            color.a = alpha;
            mpb.SetColor("_Color", color);
            mr.SetPropertyBlock(mpb);
        }
    }
}