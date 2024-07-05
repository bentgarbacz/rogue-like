using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public Dictionary<string, GameObject> projectileDictionary = new();
    public GameObject arrow;    
    public GameObject fireball;

    void Start()
    {

        projectileDictionary.Add("Arrow", arrow);
        projectileDictionary.Add("Fireball", fireball);
    }

    public GameObject CreateProjectile(string projectile, Vector3 spawnPos, Quaternion spawnRotation)
    {

        return Instantiate(projectileDictionary[projectile], spawnPos, spawnRotation);
    }
}
