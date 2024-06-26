using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public Dictionary<string, GameObject> projectileDictionary = new();
    public GameObject Arrow;    

    void Start()
    {

        projectileDictionary.Add("Arrow", Arrow);
    }

    public GameObject CreateProjectile(string projectile, Vector3 spawnPos, Quaternion spawnRotation)
    {

        return Instantiate(projectileDictionary[projectile], spawnPos, spawnRotation);
    }
}
