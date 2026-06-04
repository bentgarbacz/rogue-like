using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject focalPoint;
    public float panSpeed = 2f;
    public float minYBound = 20f;
    public float maxYBound = 40f;
    public float minFov = 10f;
    public float maxFov = 40f;
    public float zoomSensitivity = 20f;
    public string cameraDirection = "North";

    private float fov = 0;
    private float wallCheckTimer = 0f;
    private const float wallCheckInterval = 0.25f;
    private Vector3 localRotation;
    private ObjectLocation pcLocation;

    [SerializeField] private GameObject hero;
    [SerializeField] private Camera MinimapCamera;
    [SerializeField] private TileManager tileMgr;
    [SerializeField] private LogManager logMgr;


    void Start()
    {

        fov = Camera.main.fieldOfView;
        RotateCamera(Input.GetAxis("Mouse X") * panSpeed, Input.GetAxis("Mouse Y") * panSpeed);
        SetFocalPoint(hero);
        pcLocation = hero.GetComponent<ObjectLocation>();
    }

    void LateUpdate()
    {

        if (focalPoint != null) {
            
            //follow player on x and z axes, orbiting around them
            if(!focalPoint.GetComponent<AttackAnimation>().IsAttacking())
            {

                transform.position = new Vector3(
                    focalPoint.transform.position.x,
                    0,
                    focalPoint.transform.position.z
                );
            }

            
            if (Input.GetMouseButton(1)) {

                RotateCamera(Input.GetAxis("Mouse X") * panSpeed, Input.GetAxis("Mouse Y") * panSpeed);
            }

            if(Input.GetAxis("Mouse ScrollWheel") != 0 && !logMgr.IsMouseOver())
            {
                
                ZoomCamera(Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity);
            }

            // Update wall transparency based on camera position
            UpdateWallTransparency();
        }
    }

    public void RotateCamera(float xChange, float yChange)
    {      

        localRotation.x += xChange;
        localRotation.y += yChange;

        localRotation.y = Mathf.Clamp(localRotation.y, minYBound, maxYBound);

        transform.rotation = Quaternion.Euler(0f, localRotation.x, -localRotation.y);
        MinimapCamera.transform.rotation = Quaternion.Euler(270f, -(localRotation.x - 90f), 0f);
    }

    public void ZoomCamera(float fovChange)
    {

        fov -= fovChange;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    public void SetFocalPoint(GameObject focalPoint){

        this.focalPoint = focalPoint;  
    }

    private void UpdateWallTransparency()
    {

        wallCheckTimer += Time.deltaTime;

        if(wallCheckTimer < wallCheckInterval)
        {

            return;
        }

        wallCheckTimer = 0f;

        if (tileMgr == null || hero == null)
        {

            return;
        }

        // Restore all previously transparent tiles to opaque
        tileMgr.ReturnTransparentTilesToOpaque();


        // Determine cardinal/intercardinal direction based on camera rotation
        List<Vector2Int> checkDirections = new();

        // Normalize rotation angle to 0-360
        float angle = localRotation.x % 360f;
        if (angle < 0) angle += 360f;

        // Map angle to cardinal/intercardinal directions
        // 0° = North, 90° = East, 180° = South, 270° = West
        if (angle >= 337.5f || angle < 22.5f)
        {
            cameraDirection = "West";

            Vector2Int checkCoord1 = pcLocation.coord - Vector2Int.left;
            Vector2Int checkCoord2 = pcLocation.coord - new Vector2Int(-2, 0);

            if(HallwayCheck(checkCoord1, checkCoord2))
            {
                // 5x5 radius facing west
                checkDirections.Add(Vector2Int.left); 
                checkDirections.Add(new Vector2Int(-2, 0));
                checkDirections.Add(new Vector2Int(-1, 1)); 
                checkDirections.Add(new Vector2Int(-1, -1));
                checkDirections.Add(new Vector2Int(-2, 1));
                checkDirections.Add(new Vector2Int(-2, -1));
                checkDirections.Add(new Vector2Int(-1, 2));
                checkDirections.Add(new Vector2Int(-1, -2));
                checkDirections.Add(new Vector2Int(-2, 2));
                checkDirections.Add(new Vector2Int(-2, -2));
            }
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            cameraDirection = "NorthWest";
            // 5x5 radius facing northwest
            checkDirections.Add(Vector2Int.up); 
            checkDirections.Add(Vector2Int.left); 
            checkDirections.Add(new Vector2Int(-1, 1));
            checkDirections.Add(new Vector2Int(0, 2));
            checkDirections.Add(new Vector2Int(-2, 0));
            checkDirections.Add(new Vector2Int(-2, 1));
            checkDirections.Add(new Vector2Int(-1, 2));
            checkDirections.Add(new Vector2Int(-2, 2));
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            cameraDirection = "North";

            Vector2Int checkCoord1 = pcLocation.coord - Vector2Int.up;
            Vector2Int checkCoord2 = pcLocation.coord - new Vector2Int(0, 2);

            if(HallwayCheck(checkCoord1, checkCoord2))
            {
                // 5x5 radius facing north
                checkDirections.Add(Vector2Int.up); 
                checkDirections.Add(new Vector2Int(0, 2));
                checkDirections.Add(new Vector2Int(1, 1)); 
                checkDirections.Add(new Vector2Int(-1, 1));
                checkDirections.Add(new Vector2Int(1, 2));
                checkDirections.Add(new Vector2Int(-1, 2));
                checkDirections.Add(new Vector2Int(2, 1));
                checkDirections.Add(new Vector2Int(-2, 1));
                checkDirections.Add(new Vector2Int(2, 2));
                checkDirections.Add(new Vector2Int(-2, 2));
            }
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            cameraDirection = "NorthEast";
            // 5x5 radius facing northeast
            checkDirections.Add(Vector2Int.up); 
            checkDirections.Add(Vector2Int.right);
            checkDirections.Add(new Vector2Int(1, 1));
            checkDirections.Add(new Vector2Int(0, 2));
            checkDirections.Add(new Vector2Int(2, 0));
            checkDirections.Add(new Vector2Int(2, 1));
            checkDirections.Add(new Vector2Int(1, 2));
            checkDirections.Add(new Vector2Int(2, 2));
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            cameraDirection = "East";

            Vector2Int checkCoord1 = pcLocation.coord - Vector2Int.right;
            Vector2Int checkCoord2 = pcLocation.coord - new Vector2Int(2, 0);

            if(HallwayCheck(checkCoord1, checkCoord2))
            {
                // 5x5 radius facing east
                checkDirections.Add(Vector2Int.right); 
                checkDirections.Add(new Vector2Int(2, 0));
                checkDirections.Add(new Vector2Int(1, 1)); 
                checkDirections.Add(new Vector2Int(1, -1));
                checkDirections.Add(new Vector2Int(2, 1));
                checkDirections.Add(new Vector2Int(2, -1));
                checkDirections.Add(new Vector2Int(1, 2));
                checkDirections.Add(new Vector2Int(1, -2));
                checkDirections.Add(new Vector2Int(2, 2));
                checkDirections.Add(new Vector2Int(2, -2));
            }
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            cameraDirection = "SouthEast";
            // 5x5 radius facing southeast
            checkDirections.Add(Vector2Int.down); 
            checkDirections.Add(Vector2Int.right);
            checkDirections.Add(new Vector2Int(1, -1));
            checkDirections.Add(new Vector2Int(0, -2));
            checkDirections.Add(new Vector2Int(2, 0));
            checkDirections.Add(new Vector2Int(2, -1));
            checkDirections.Add(new Vector2Int(1, -2));
            checkDirections.Add(new Vector2Int(2, -2));
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            cameraDirection = "South";

            Vector2Int checkCoord1 = pcLocation.coord - Vector2Int.down;
            Vector2Int checkCoord2 = pcLocation.coord - new Vector2Int(0, -2);

            if(HallwayCheck(checkCoord1, checkCoord2))
            {
                // 5x5 radius facing south
                checkDirections.Add(Vector2Int.down);
                checkDirections.Add(new Vector2Int(0, -2));
                checkDirections.Add(new Vector2Int(1, -1));
                checkDirections.Add(new Vector2Int(-1, -1));
                checkDirections.Add(new Vector2Int(1, -2));
                checkDirections.Add(new Vector2Int(-1, -2));
                checkDirections.Add(new Vector2Int(2, -1));
                checkDirections.Add(new Vector2Int(-2, -1));
                checkDirections.Add(new Vector2Int(2, -2));
                checkDirections.Add(new Vector2Int(-2, -2));
            }
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            cameraDirection = "SouthWest";
            // 5x5 radius facing southwest
            checkDirections.Add(Vector2Int.left);
            checkDirections.Add(Vector2Int.down);
            checkDirections.Add(new Vector2Int(-1, -1));
            checkDirections.Add(new Vector2Int(0, -2));
            checkDirections.Add(new Vector2Int(-2, 0));
            checkDirections.Add(new Vector2Int(-2, -1));
            checkDirections.Add(new Vector2Int(-1, -2));
            checkDirections.Add(new Vector2Int(-2, -2));
        }

        // Make tiles transparent in the calculated directions
        foreach (Vector2Int direction in checkDirections)
        {

            Vector2Int checkCoord = pcLocation.coord - direction;

            if (tileMgr.tileDict.ContainsKey(checkCoord))
            {

                Tile tile = tileMgr.tileDict[checkCoord];

                // Only make walls transparent
                if (!tile.IsActionable())
                {

                    TransparencyManager.SetTransparency(tile.tileRenderer, 0.3f);
                    tileMgr.transparentTiles.Add(checkCoord);
                }
            }
        }
    }

    private bool HallwayCheck(Vector2Int checkCoord1, Vector2Int checkCoord2)
    {

        bool bool1 = false;
        bool bool2 = false;


        if(tileMgr.tileDict.ContainsKey(checkCoord1))
        {

            bool1 = tileMgr.tileDict[checkCoord1].IsActionable();
        }

        if(tileMgr.tileDict.ContainsKey(checkCoord2))
        {

            bool2 = tileMgr.tileDict[checkCoord2].IsActionable();
        }

        return !(bool1 && bool2);
    }
}

