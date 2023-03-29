using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMap : MonoBehaviour
{
    //Get player script
    //Get list of active bots
    private PlayerMovement player;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        cameras = GameObject.FindGameObjectsWithTag("Camera");
        GameObject[] tempScramblers = GameObject.FindGameObjectsWithTag("Scrambler");
        foreach(GameObject scrambler in tempScramblers)
        {
            scramblers.Add(scrambler.GetComponent<Scrambler>());
        }

        SetScrambledTiles();

        Scrambler.scramblerToggled += SetScrambledTiles;

        GenerateMap();
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
        GridPosition playerPosition = LevelGrid.Instance.GetGridPosition(player.transform.position);
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
            GridPosition gridDistanceFromActiveCam = currentGridPosition - activeCameraGridPosition;
            if (currentGridPosition == playerPosition)
            {
                tile.SetImage(playerSprite);
            }
            else if((Mathf.Abs(gridDistanceFromActiveCam.x) + Mathf.Abs(gridDistanceFromActiveCam.z)) > cameraMapViewRange)
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
        if (enable)
        {
            foreach(GridMapTile tile in mapTiles)
            {
                GridPosition currentGridPosition = tile.GetGridPosition();
                //test to see if tile is in teleport range, is walkable and valid, isn't scrambled. If yes then enable the button.
                //Also change teleport button to be cancel button instead
            }
        }
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
