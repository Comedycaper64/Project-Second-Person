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
    private GridPosition activeCameraGridPosition;

    [SerializeField] private float mapRefreshTime;
    [SerializeField] private float cameraMapViewRange;

    [SerializeField] private GameObject walkableTile;
    [SerializeField] private GameObject unWalkableTile;
    [SerializeField] private GameObject unseenTile;
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
                if (currentGridPosition == playerPosition)
                {
                    Instantiate(playerTile, mapContainer);
                }
                else if((Mathf.Abs(gridDistanceFromActiveCam.x) + Mathf.Abs(gridDistanceFromActiveCam.z)) > cameraMapViewRange)
                {
                    Instantiate(unseenTile, mapContainer);
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
