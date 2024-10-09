using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileReferences : MonoBehaviour
{

    public Dictionary<ProjectileType, GameObject> projectileDict;
    [SerializeField] private GameObject arrow;    
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject magicMissile;

    void Start()
    {

        projectileDict = new()
        {
            {ProjectileType.Arrow, arrow},
            {ProjectileType.Fireball, fireball},
            {ProjectileType.MagicMissile, magicMissile}
        };
    }

    public GameObject CreateProjectile(ProjectileType projType, Vector3 spawnPos, Quaternion spawnRotation)
    {

        return Instantiate(projectileDict[projType], spawnPos, spawnRotation);
    }
}