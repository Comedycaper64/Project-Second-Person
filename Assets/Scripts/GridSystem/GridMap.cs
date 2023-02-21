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

    private int mapWidth;
    private int mapHeight;
    private Transform mapContainer;
    private GridLayoutGroup layoutGroup;

    [SerializeField] private float mapRefreshTime;

    [SerializeField] private GameObject walkableTile;
    [SerializeField] private GameObject unWalkableTile;
    [SerializeField] private GameObject playerTile;
    [SerializeField] private GameObject cameraTile;

    private void Start() 
    {
        mapWidth = LevelGrid.Instance.GetWidth();
        mapHeight = LevelGrid.Instance.GetHeight();
        mapContainer = this.gameObject.transform;
        layoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
        layoutGroup.constraintCount = mapWidth;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        cameras = GameObject.FindGameObjectsWithTag("Camera");

        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap()
    {   
        ClearMapContainer();
        GridPosition playerPosition = LevelGrid.Instance.GetGridPosition(player.transform.position);
        //Debug.Log(playerPosition);
        List<GridPosition> cameraPositions = new List<GridPosition>();
        foreach(GameObject camera in cameras)
        {
            cameraPositions.Add(LevelGrid.Instance.GetGridPosition(camera.transform.position));
        }

        for(int x = 0; x < mapWidth; x++)
        {
            for(int z = 0; z < mapHeight; z++)
            {
                GridPosition currentGridPosition = new GridPosition(x, z);
                if (currentGridPosition == playerPosition)
                {
                    Instantiate(playerTile, mapContainer);
                }
                else if (cameraPositions.Contains(currentGridPosition))
                {
                    Instantiate(cameraTile, mapContainer);
                }
                else if (LevelGrid.Instance.IsWalkableGridPosition(currentGridPosition))
                {
                    Instantiate(walkableTile, mapContainer);
                }
                else
                {
                    Instantiate(unWalkableTile, mapContainer);
                }
            }
        }

        yield return new WaitForSeconds(mapRefreshTime);

        StartCoroutine(GenerateMap());
    }

    private void ClearMapContainer()
    {
        foreach(Transform child in mapContainer)
        {
            Destroy(child.gameObject);
        }
    }

}
