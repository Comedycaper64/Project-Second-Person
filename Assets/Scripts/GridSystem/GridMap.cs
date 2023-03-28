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
    private GridLayoutGroup layoutGroup;
    private GridPosition activeCameraGridPosition;

    [SerializeField] private float mapRefreshTime;
    [SerializeField] private float cameraMapViewRange;

    [SerializeField] private GameObject walkableTile;
    [SerializeField] private GameObject unWalkableTile;
    [SerializeField] private GameObject unseenTile;
    [SerializeField] private GameObject scrambledTile;
    [SerializeField] private GameObject playerTile;
    [SerializeField] private GameObject cameraTile;

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

        StartCoroutine(GenerateMap());
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

    private IEnumerator GenerateMap()
    {   
        ClearMapContainer();
        GridPosition playerPosition = LevelGrid.Instance.GetGridPosition(player.transform.position);
        //Debug.Log(playerPosition);
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

        for(int x = 0; x < mapWidth; x++)
        {
            for(int z = 0; z < mapHeight; z++)
            {
                GridPosition currentGridPosition = new GridPosition(x, z);
                GridPosition gridDistanceFromActiveCam = currentGridPosition - activeCameraGridPosition;
                GameObject spawnedTile;
                if (currentGridPosition == playerPosition)
                {
                    spawnedTile = Instantiate(playerTile, mapContainer);
                }
                else if((Mathf.Abs(gridDistanceFromActiveCam.x) + Mathf.Abs(gridDistanceFromActiveCam.z)) > cameraMapViewRange)
                {
                    spawnedTile = Instantiate(unseenTile, mapContainer);
                }
                else if (LevelGrid.Instance.GetGridObject(currentGridPosition).IsScrambled())
                {
                    spawnedTile = Instantiate(scrambledTile, mapContainer);
                }
                else if (cameraPositions.Contains(currentGridPosition))
                {
                    spawnedTile = Instantiate(cameraTile, mapContainer);
                }
                else if (LevelGrid.Instance.IsWalkableGridPosition(currentGridPosition))
                {
                    spawnedTile = Instantiate(walkableTile, mapContainer);
                }
                else
                {
                    spawnedTile = Instantiate(unWalkableTile, mapContainer);
                }
                spawnedTile.transform.localScale = new Vector3(mapTileScale, mapTileScale, mapTileScale);
            }
        }

        yield return new WaitForSeconds(mapRefreshTime);

        StartCoroutine(GenerateMap());
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
