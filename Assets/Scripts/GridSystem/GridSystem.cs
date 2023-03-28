using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    //**The Grid**
    private int width;  //How wide it is
    private int height; //How high it is
    private float cellSize; //How large each tile is
    private GridObject[,] gridObjectArray;  //All of the tiles on this grid, stored in a 2D array based on what their gridposition is

    public GridSystem(int width, int height, float cellSize)
    //Constructor
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {     
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);  
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.FloorToInt(worldPosition.x / cellSize),
            Mathf.FloorToInt(worldPosition.z / cellSize)
        );  
    }

    //Makes all the GridDebugObjects to show the GridPosition + current units of all tiles
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetCellCenter(gridPosition), Quaternion.identity);
            }
        }

    }

    //You can get a specific tile based on its GridPosition :ooo what a good 2D array that is
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool CheckIfTileExists(GridPosition gridPosition)
    {
        float tileCheckRadius = .5f;
        Collider[] colliderArray = Physics.OverlapSphere(GetCellCenter(gridPosition), tileCheckRadius);
        foreach (Collider collider in colliderArray)
        {
            if (collider.gameObject.layer == 6)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsWithinGrid(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
                gridPosition.z >= 0 && 
                gridPosition.x < width && 
                gridPosition.z < height;
    }

    //Tests if the tile is within the Grid
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return IsWithinGrid(gridPosition) && CheckIfTileExists(gridPosition);
    }

    public int GetWidth() //of the Grid
    {
        return width;
    }

    public int GetHeight()//^
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetCellCenter(GridPosition gridPosition)
    {
        return (GetWorldPosition(gridPosition) + new Vector3(cellSize / 2, 0, cellSize / 2));
    }

}
