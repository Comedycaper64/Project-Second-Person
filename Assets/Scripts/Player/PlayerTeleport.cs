using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private int totalTeleports;
    private int availableTeleports;
    private PlayerMovement movement;


    private void Awake() 
    {
        availableTeleports = totalTeleports;    
        movement = GetComponent<PlayerMovement>();
        GridMapTile.OnTilePressed += Teleport;
    }

    public void Teleport(object sender, GridPosition gridPosition)
    {
        if ((availableTeleports > 0) && LevelGrid.Instance.IsValidGridPosition(gridPosition) && !LevelGrid.Instance.GetGridObject(gridPosition).IsScrambled())
        {
            availableTeleports--;
            movement.ToggleController(false);
            transform.position = LevelGrid.Instance.GetCellCentre(gridPosition);
            movement.ToggleController(true);
            movement.ToggleMovement(true);
            
        }
    }
}
