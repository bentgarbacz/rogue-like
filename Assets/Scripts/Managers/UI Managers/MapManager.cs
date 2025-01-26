using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private GameObject tileMarker;
    [SerializeField] private GameObject objectMarker;
    private UIActiveManager uiam;

    // Start is called before the first frame update
    void Start()
    {
        
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
