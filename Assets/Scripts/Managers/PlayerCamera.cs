using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject focalPoint;
    public float panSpeed = 2f;
    private Vector3 localRotation;
    public float minYBound = 20f;
    public float maxYBound = 40f;
    public float minFov = 10f;
    public float maxFov = 40f;
    public float zoomSensitivity = 20f;
    private float fov = 0;
    [SerializeField] GameObject hero;
    [SerializeField] Camera MinimapCamera;
    [SerializeField] private TileManager tileMgr;
    public string cameraDirection = "North";

    void Start()
    {

        fov = Camera.main.fieldOfView;
        RotateCamera(Input.GetAxis("Mouse X") * panSpeed, Input.GetAxis("Mouse Y") * panSpeed);
        SetFocalPoint(hero);
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

            if(Input.GetAxis("Mouse ScrollWheel") != 0)
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

        if (tileMgr == null || hero == null)
        {

            return;
        }

        // Restore all previously transparent tiles to opaque
        tileMgr.ReturnTransparentTilesToOpaque();

        // Get hero position
        Vector3 heroPos = hero.transform.position;
        Vector2Int heroCoord = new Vector2Int((int)heroPos.x, (int)heroPos.z);

        // Determine cardinal/intercardinal direction based on camera rotation
        List<Vector2Int> directionsToCheck = new();

        // Normalize rotation angle to 0-360
        float angle = localRotation.x % 360f;
        if (angle < 0) angle += 360f;

        // Map angle to cardinal/intercardinal directions
        // 0° = North, 90° = East, 180° = South, 270° = West
        if (angle >= 337.5f || angle < 22.5f)
        {
            cameraDirection = "West";

            if(!tileMgr.tileDict[heroCoord - Vector2Int.left].IsActionable())
            {
                directionsToCheck.Add(Vector2Int.left); 
                directionsToCheck.Add(new Vector2Int(-1, 1)); 
                directionsToCheck.Add(new Vector2Int(-1, -1)); 
            }
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            cameraDirection = "NorthWest";
            directionsToCheck.Add(Vector2Int.up); 
            directionsToCheck.Add(Vector2Int.left); 
            directionsToCheck.Add(new Vector2Int(-1, 1)); 
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            cameraDirection = "North";
            
            if(!tileMgr.tileDict[heroCoord - Vector2Int.up].IsActionable())
            {

                directionsToCheck.Add(Vector2Int.up); 
                directionsToCheck.Add(new Vector2Int(1, 1)); 
                directionsToCheck.Add(new Vector2Int(-1, 1));
            }
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            cameraDirection = "NorthEast";
            directionsToCheck.Add(Vector2Int.up); 
            directionsToCheck.Add(new Vector2Int(1, 1)); 
            directionsToCheck.Add(Vector2Int.right);  
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            cameraDirection = "East";
            
            if(!tileMgr.tileDict[heroCoord - Vector2Int.right].IsActionable())
            {

                directionsToCheck.Add(Vector2Int.right); 
                directionsToCheck.Add(new Vector2Int(1, 1)); 
                directionsToCheck.Add(new Vector2Int(1, -1)); 
            }
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            cameraDirection = "SouthEast";
            directionsToCheck.Add(Vector2Int.down); 
            directionsToCheck.Add(Vector2Int.right); 
            directionsToCheck.Add(new Vector2Int(1, -1)); 
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            cameraDirection = "South";

            if(!tileMgr.tileDict[heroCoord - Vector2Int.down].IsActionable())
            {

                directionsToCheck.Add(Vector2Int.down);
                directionsToCheck.Add(new Vector2Int(1, -1));
                directionsToCheck.Add(new Vector2Int(-1, -1));
            }
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            cameraDirection = "SouthWest";
            directionsToCheck.Add(Vector2Int.left);
            directionsToCheck.Add(new Vector2Int(-1, -1));
            directionsToCheck.Add(Vector2Int.down);
        }

        // Make tiles transparent in the calculated directions
        foreach (Vector2Int direction in directionsToCheck)
        {

            Vector2Int checkCoord = heroCoord - direction;

            if (tileMgr.tileDict.ContainsKey(checkCoord))
            {

                Tile tile = tileMgr.tileDict[checkCoord];

                // Only make walls transparent
                if (!tile.IsActionable())
                {

                    Renderer renderer = tile.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {

                        TransparencyManager.SetTransparency(renderer, 0.3f);
                        tileMgr.transparentTiles.Add(checkCoord);
                    }
                }
            }
        }
    }
}

