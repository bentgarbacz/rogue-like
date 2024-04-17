using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    Tile northTile;
    Tile eastTile;
    Tile southTile;
    Tile westTile;
    string color;

    public Tile(string clr){
        color = clr;
    }
    
    public void print(){
        Debug.Log(color);
    }
}
