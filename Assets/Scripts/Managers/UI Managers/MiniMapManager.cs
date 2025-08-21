using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{

    [SerializeField] private GameObject map;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Sprite enemyIcon;
    [SerializeField] private Sprite pcIcon;
    [SerializeField] private Sprite objectIcon;
    [SerializeField] private Sprite tileIcon;
    [SerializeField] private Sprite entranceIcon;
    [SerializeField] private Sprite exitIcon;
    [SerializeField] private Camera mapCamera;
    public HashSet<GameObject> iconGameObjects = new();
    private List<MapIconController> dynamicObjectIcons;
    private Dictionary<Vector2Int, MapIconController> tileIcons;
    private EntityManager entityMgr;
    private MapIconController anchorIcon;
    private static int tileLayer = 1;
    private static int objectLayer = 2;
    private static int characterLayer = 3;
    private static int pcLayer = 5;

    void Start()
    {  

        GameObject managers = GameObject.Find("System Managers");

        entityMgr = managers.GetComponent<EntityManager>();
    }

    public void RevealTile(Vector2Int coord)
    {

        tileIcons[coord].SetVisibility(true);
    }

    public void DrawIcons(HashSet<GameObject> objects)
    {

        dynamicObjectIcons = new();
        tileIcons = new();

        foreach (GameObject currentObject in objects)
        {

            if (currentObject.GetComponent<Tile>() is Tile t)
            {

                AddIcon(currentObject);
            }
        }

        foreach (GameObject currentObject in objects)
        {

            if (currentObject.GetComponent<Loot>() is Loot l)
            {

                AddIcon(currentObject);
            }
        }

        foreach (GameObject currentObject in objects)
        {

            if (currentObject.GetComponent<CharacterSheet>() is CharacterSheet cs)
            {

                AddIcon(currentObject);
            }
        }

        GameObject newIconPC = Instantiate(iconPrefab, map.transform);
        iconGameObjects.Add(newIconPC);
        anchorIcon = newIconPC.GetComponent<MapIconController>();

        dynamicObjectIcons.Add(anchorIcon);

        anchorIcon.InitializeController(entityMgr.hero, pcIcon, entityMgr.playerCharacter.loc.coord, pcLayer, 0.66f);
    }

    public void AddIcon(GameObject newObject)
    {

        GameObject newIcon = Instantiate(iconPrefab, map.transform);
        MapIconController mic = newIcon.GetComponent<MapIconController>();

        if (newObject.GetComponent<Tile>() is Tile currentTile)
        {

            if (currentTile.GetIconType() == IconType.Entrance)
            {

                mic.InitializeController(newObject, entranceIcon, currentTile.loc.coord, tileLayer, 1.33f);

            }
            else if (currentTile.GetIconType() == IconType.Tile)
            {

                mic.InitializeController(newObject, tileIcon, currentTile.loc.coord, tileLayer);

            }
            else if (currentTile.GetIconType() == IconType.Wall)
            {

                mic.InitializeController(newObject, tileIcon, currentTile.loc.coord, tileLayer);
                Color currentColor = mic.Icon().color;
                mic.Icon().color = new Color(currentColor.r * 0.5f, currentColor.g * 0.5f, currentColor.b * 0.5f, currentColor.a);

            }
            else if (currentTile.GetIconType() == IconType.Exit)
            {

                mic.InitializeController(newObject, exitIcon, currentTile.loc.coord, tileLayer, 1.33f);
            }

            tileIcons.Add(currentTile.loc.coord, mic);          

        }
        else if (newObject.GetComponent<Loot>() is Loot currentLoot)
        {

            mic.InitializeController(newObject, objectIcon, currentLoot.loc.coord, objectLayer, 0.66f);
            dynamicObjectIcons.Add(mic);

        }
        else if (newObject.GetComponent<CharacterSheet>() is CharacterSheet currentCharacter)
        {

            if (currentCharacter is EnemyCharacterSheet ec)
            {

                mic.SetIcon(enemyIcon);
                mic.InitializeController(newObject, enemyIcon, currentCharacter.loc.coord, characterLayer, 0.66f);
            }

            dynamicObjectIcons.Add(mic);

        }
        else
        {

            Destroy(newIcon);
            return;
        }

        iconGameObjects.Add(newIcon);

        if (newObject.GetComponent<ObjectVisibility>() is ObjectVisibility ov)
        {

            ov.SetIcon(mic);
        }

        mic.SetVisibility(false);
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

        // y value is constant to preserve camera height
        // z value is negated because the camera and the map are facing eachother, like a mirror
        mapCamera.transform.position = new Vector3(

            anchorIcon.iconRectTransform.anchoredPosition.x, 
            mapCamera.transform.position.y, 
            -anchorIcon.iconRectTransform.anchoredPosition.y
        );
    }
}
