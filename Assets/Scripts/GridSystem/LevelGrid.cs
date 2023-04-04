using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    //Static-ed for obvious reasons
    public static LevelGrid Instance {get; private set;}
    [SerializeField] private Transform gridDebugObjectPrefab;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem gridSystem;

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Gridsystem is created
        gridSystem = new GridSystem(width, height, cellSize);
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {     
                GridPosition gridPosition = new GridPosition(x, z);
                GetGridObject(gridPosition).SetIsWalkable(gridSystem.CheckIfTileExists(gridPosition));
            }
        }
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    //This script is basically for other classes to interact with the GridSystem and to see/manage which tiles units are on

    public bool HasGridObject(GridPosition gridPosition)
    {
        if (gridSystem.GetGridObject(gridPosition) != null)
        {
            return true;
        }
        return false;
    }


    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public GridObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public bool IsWithinGrid(GridPosition gridPosition) => gridSystem.IsWithinGrid(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    
    public int GetHeight() => gridSystem.GetHeight();

    public float GetCellSize() => gridSystem.GetCellSize();

    public Vector3 GetCellCentre(GridPosition gridPosition) => gridSystem.GetCellCenter(gridPosition);

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        if (!IsValidGridPosition(gridPosition)) {return false;}

        GridObject gridObject = GetGridObject(gridPosition);
        return gridObject.IsWalkable();
    }

    // public bool HasCameraGridPosition(GridPosition gridPosition)
    // {
    //     if (!IsValidGridPosition(gridPosition)) {return false;}

    //     GridObject gridObject = GetGridObject(gridPosition);
    //     return gridObject.HasCamera();
    // }
}
