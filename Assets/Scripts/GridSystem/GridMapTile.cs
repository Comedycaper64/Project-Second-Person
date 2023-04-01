using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMapTile : MonoBehaviour
{
    private Image tileImage;
    private Button tileButton;
    private GridPosition tileGridPosition;

    public static EventHandler<GridPosition> OnTilePressed;

    private void Awake() 
    {
        tileImage = GetComponent<Image>();    
        tileButton = GetComponent<Button>();
        ToggleButton(false);
    }

    public void ToggleButton(bool enable)
    {
        tileButton.interactable = enable;
    }

    public GridPosition GetGridPosition()
    {
        return tileGridPosition;
    }

    public void SetGridPosition(GridPosition gridPosition)
    {
        tileGridPosition = gridPosition;
    }

    public void SetImage(Sprite tile)
    {
        tileImage.sprite = tile;
    }

    public void Teleport()
    {
        OnTilePressed.Invoke(this, tileGridPosition);
    }
}
