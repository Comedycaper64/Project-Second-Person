using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    //Each tile on the Grid is a GridObject
    private GridSystem gridSystem;
    private bool isWalkable;
    private bool hasCamera;
    private GridPosition gridPosition;  //Position of this tile

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)  //Constructor
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
    }

    public GridPosition GetGridPosition()
    {
        return this.gridPosition;
    }

    // public bool HasCamera()
    // {
    //     return hasCamera;
    // }

    // public void SetHasCamera(bool hasCamera)
    // {
    //     this.hasCamera = hasCamera;
    // }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
