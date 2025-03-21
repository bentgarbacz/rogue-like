using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private Canvas mapCanvas;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Sprite enemyIcon;
    [SerializeField] private Sprite pcIcon;
    [SerializeField] private Sprite objectIcon;
    [SerializeField] private Sprite tileIcon;
    [SerializeField] private Sprite entranceIcon;
    [SerializeField] private Sprite exitIcon;
    private List<MapIconController> dynamicObjectIcons = new();
    private DungeonManager dum;

    void Start()
    {
        
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
    }

    public void RevealTiles(Vector2Int coord)
    {


    }

    public void DrawIcons(HashSet<GameObject> objects)
    {

        foreach(GameObject currentObject in objects)
        {
            if(currentObject.GetComponent<Tile>() is Tile currentTile)
            {
                AddIcon(currentObject);
            }
        }

        foreach(GameObject currentObject in objects)
        {

            if(currentObject.GetComponent<Loot>() is Loot currentLoot)
            {
                AddIcon(currentObject);
            }
        }

        foreach(GameObject currentObject in objects)
        {

            if(currentObject.GetComponent<CharacterSheet>() is CharacterSheet currentCharacter)
            {
                AddIcon(currentObject);
            }
        }

        GameObject newIconPC = Instantiate(iconPrefab, mapCanvas.transform);
        MapIconController micPC = newIconPC.GetComponent<MapIconController>();

        dynamicObjectIcons.Add(micPC);

        micPC.InitializeController(dum.hero, pcIcon, dum.playerCharacter.coord, 5, 0.66f);
    }

    public void AddIcon(GameObject newObject)
    {

        GameObject newIcon = Instantiate(iconPrefab, mapCanvas.transform);
        MapIconController mic = newIcon.GetComponent<MapIconController>();

        if(newObject.GetComponent<Tile>() is Tile currentTile)
        {
            
            if(currentTile.GetIconType() == IconType.Entrance)
            {

                mic.InitializeController(newObject, entranceIcon, currentTile.coord, 0);

            }else if(currentTile.GetIconType() == IconType.Tile)
            {

                mic.InitializeController(newObject, tileIcon, currentTile.coord, 0);

            }else if(currentTile.GetIconType() == IconType.Wall)
            {

                mic.InitializeController(newObject, tileIcon, currentTile.coord, 0);
                Color currentColor = mic.Icon().color;
                mic.Icon().color = new Color(currentColor.r * 0.5f, currentColor.g * 0.5f, currentColor.b * 0.5f, currentColor.a);

            }

        }else if(newObject.GetComponent<Loot>() is Loot currentLoot)
        {

            mic.InitializeController(newObject, objectIcon, currentLoot.coord, 3, 0.66f);
            dynamicObjectIcons.Add(mic);

        }else if(newObject.GetComponent<CharacterSheet>() is CharacterSheet currentCharacter)
        {

            if(currentCharacter is EnemyCharacterSheet ec)
            {

                mic.SetIcon(enemyIcon);
                mic.InitializeController(newObject, enemyIcon, currentCharacter.coord, 4, 0.66f);
            }

            dynamicObjectIcons.Add(mic);

        }else if(newObject.GetComponent<Exit>() is Exit currentExit)
        {

            mic.InitializeController(newObject, exitIcon, currentExit.coord, 2, 1.66f);

        }else
        {

            Destroy(newIcon);
            return;
        }

        dum.iconGameObjects.Add(newIcon);
    }

    public void UpdateDynamicIcons()
    {

        for (int i = dynamicObjectIcons.Count - 1; i >= 0; i--)
        {

            if (!dynamicObjectIcons[i].UpdateIcon())
            {

                dynamicObjectIcons.RemoveAt(i);
            }
        }
    }
}
