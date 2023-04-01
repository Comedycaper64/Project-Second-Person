using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMap : MonoBehaviour
{
    //Get player script
    //Get list of active bots
    private PlayerMovement playerMovement;
    private PlayerTeleport playerTeleport;
    private GameObject[] cameras;
    [SerializeField] private List<Scrambler> scramblers = new List<Scrambler>();

    private int mapWidth;
    private int mapHeight;
    private float mapSize;
    private float mapTileScale;
    private Transform mapContainer;
    private List<GridMapTile> mapTiles = new List<GridMapTile>();
    private GridLayoutGroup layoutGroup;
    private GridPosition activeCameraGridPosition;

    [SerializeField] private GameObject teleportButton;
    [SerializeField] private GameObject cancelButton;

    [SerializeField] private float mapRefreshTime;
    [SerializeField] private float cameraMapViewRange;

    [SerializeField] private GameObject mapTile;
    [SerializeField] private Sprite unWalkableSprite;
    [SerializeField] private Sprite walkableSprite;
    [SerializeField] private Sprite unseenSprite;
    [SerializeField] private Sprite scrambledSprite;
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Sprite cameraSprite;

    private void Start() 
    {
        mapWidth = LevelGrid.Instance.GetWidth();
        mapHeight = LevelGrid.Instance.GetHeight();
        mapTileScale = 20f / mapWidth;
        mapSize = 40f - mapWidth;
        mapContainer = this.gameObject.transform;
        layoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
        layoutGroup.constraintCount = mapWidth;
        layoutGroup.cellSize = new Vector2(mapSize, mapSize);
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerTeleport = playerMovement.GetComponent<PlayerTeleport>();
        cameras = GameObject.FindGameObjectsWithTag("Camera");
        GameObject[] tempScramblers = GameObject.FindGameObjectsWithTag("Scrambler");
        foreach(GameObject scrambler in tempScramblers)
        {
            scramblers.Add(scrambler.GetComponent<Scrambler>());
        }

        SetScrambledTiles();

        Scrambler.scramblerToggled += SetScrambledTiles;
        GridMapTile.OnTilePressed += DisableTeleportUI;

        GenerateMap();
        ToggleTeleportUI(false);
    }

    private void SetScrambledTiles()
    {
        for(int x = 0; x < mapWidth; x++)
        {
            for(int z = 0; z < mapHeight; z++)
            {
                LevelGrid.Instance.GetGridObject(new GridPosition(x,z)).SetScrambled(false);
            }
        }

        foreach(Scrambler scrambler in scramblers)
        {
            if (!scrambler.scramblerEnabled) {continue;}

            for (int i = scrambler.scramblerPosition.x - scrambler.scramblerRange; i <= scrambler.scramblerPosition.x + scrambler.scramblerRange; i++)
            {
                for (int j = scrambler.scramblerPosition.z - scrambler.scramblerRange; j <= scrambler.scramblerPosition.z + scrambler.scramblerRange; j++)
                {
                    GridPosition gridPosition = new GridPosition(i,j);
                    if (LevelGrid.Instance.IsWithinGrid(gridPosition))
                    {
                        GridPosition distanceFromScrambler = gridPosition - scrambler.scramblerPosition;
                        if ((Mathf.Abs(distanceFromScrambler.x) + Mathf.Abs(distanceFromScrambler.z)) <= scrambler.scramblerRange)
                        {
                            LevelGrid.Instance.GetGridObject(gridPosition).SetScrambled(true);
                        }
                    }
                }
            }
        }
    }

    private void GenerateMap()
    {
        ClearMapContainer();
        for(int x = 0; x < mapWidth; x++)
        {
            for(int z = 0; z < mapHeight; z++)
            {
                GridPosition currentGridPosition = new GridPosition(x, z);
                GridMapTile spawnedTile = Instantiate(mapTile, mapContainer).GetComponent<GridMapTile>();
                spawnedTile.transform.localScale = new Vector3(mapTileScale, mapTileScale, mapTileScale);
                spawnedTile.SetGridPosition(currentGridPosition);
                mapTiles.Add(spawnedTile);
            }
        }

        StartCoroutine(UpdateMap());
    }

    private IEnumerator UpdateMap()
    {   
        GridPosition playerPosition = LevelGrid.Instance.GetGridPosition(playerMovement.transform.position);
        List<GridPosition> cameraPositions = new List<GridPosition>();
        foreach(GameObject camera in cameras)
        {
            GridPosition cameraPosition = LevelGrid.Instance.GetGridPosition(camera.transform.position);
            cameraPositions.Add(cameraPosition);
            if (camera.GetComponent<CameraObject>().IsCameraActive())
            {
                activeCameraGridPosition = cameraPosition;
            }
        }

        foreach(GridMapTile tile in mapTiles)
        {
            GridPosition currentGridPosition = tile.GetGridPosition();

            if (currentGridPosition == playerPosition)
            {
                tile.SetImage(playerSprite);
            }
            else if(!TileInViewRange(currentGridPosition))
            {
                tile.SetImage(unseenSprite);
            }
            else if (LevelGrid.Instance.GetGridObject(currentGridPosition).IsScrambled())
            {
                tile.SetImage(scrambledSprite);
            }
            else if (cameraPositions.Contains(currentGridPosition))
            {
                tile.SetImage(cameraSprite);
            }
            else if (LevelGrid.Instance.IsWalkableGridPosition(currentGridPosition))
            {
                tile.SetImage(walkableSprite);
            }
            else
            {
                tile.SetImage(unWalkableSprite);
            }
        }

        yield return new WaitForSeconds(mapRefreshTime);

        StartCoroutine(UpdateMap());
    }

    public void ToggleTeleportUI(bool enable)
    {
        if ((playerTeleport.GetAvailableTeleports() == 0) && enable)
        {
            return;
        }

        teleportButton.SetActive(!enable);
        cancelButton.SetActive(enable);
        playerMovement.ToggleMovement(!enable);

        if (enable)
        {
            foreach(GridMapTile tile in mapTiles)
            {
                GridPosition currentGridPosition = tile.GetGridPosition();
                GridPosition playerPosition = LevelGrid.Instance.GetGridPosition(playerMovement.transform.position);
                GridPosition tileDistanceFromPlayerPosition = currentGridPosition - playerPosition;
                if ((currentGridPosition == playerPosition) 
                    || !TileInViewRange(currentGridPosition)
                    || ((Mathf.Abs(tileDistanceFromPlayerPosition.x) + Mathf.Abs(tileDistanceFromPlayerPosition.z)) > playerTeleport.GetTeleportRange()) 
                    || !LevelGrid.Instance.IsValidGridPosition(currentGridPosition) 
                    || LevelGrid.Instance.GetGridObject(currentGridPosition).IsScrambled())
                {
                    continue;
                }
                else 
                {
                    tile.ToggleButton(true);
                }
            }
        }
        else
        {
            foreach(GridMapTile tile in mapTiles)
            {
                tile.ToggleButton(false);
            }
        }
    }

    private bool TileInViewRange(GridPosition gridPosition)
    {
        GridPosition gridDistanceFromActiveCam = gridPosition - activeCameraGridPosition;
        return ((Mathf.Abs(gridDistanceFromActiveCam.x) + Mathf.Abs(gridDistanceFromActiveCam.z)) <= cameraMapViewRange);
    }

    private void DisableTeleportUI(object sender, GridPosition e)
    {
        ToggleTeleportUI(false);
    }

    private void OnDestroy() 
    {
        Scrambler.scramblerToggled -= SetScrambledTiles;   
    }

    private void ClearMapContainer()
    {
        foreach(Transform child in mapContainer)
        {
            Destroy(child.gameObject);
        }
    }

}
