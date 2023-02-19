using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    //Static-ed for obvious reasons
    public static LevelGrid Instance {get; private set;}

    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private Transform gridDebugObjectPrefab;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

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
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, 
                (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));

        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
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

    public int GetWidth() => gridSystem.GetWidth();
    
    public int GetHeight() => gridSystem.GetHeight();

    public float GetCellSize() => gridSystem.GetCellSize();

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        if (!IsValidGridPosition(gridPosition)) {return false;}

        GridObject gridObject = GetGridObject(gridPosition);
        return gridObject.IsWalkable();
    }

    public bool HasCameraGridPosition(GridPosition gridPosition)
    {
        if (!IsValidGridPosition(gridPosition)) {return false;}

        GridObject gridObject = GetGridObject(gridPosition);
        return gridObject.HasCamera();
    }

    // public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    // {
    //     if (!HasGridObject(gridPosition))
    //         return false;    
    //     GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //     return gridObject.HasAnyUnit();
    // }

    // public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    // {
    //     GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //     return gridObject.GetUnit();
    // }

    // public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    // {
    //     GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //     return gridObject.GetInteractable();
    // }

    // public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    // {
    //     GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //     gridObject.SetInteractable(interactable);
    // }
}
