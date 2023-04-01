using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private int totalTeleports;
    [SerializeField] private int teleportRange;
    private int availableTeleports;
    private PlayerMovement movement;


    private void Awake() 
    {
        availableTeleports = totalTeleports;    
        movement = GetComponent<PlayerMovement>();
        GridMapTile.OnTilePressed += Teleport;
    }

    public int GetTeleportRange()
    {
        return teleportRange;
    }

    public int GetAvailableTeleports()
    {
        return availableTeleports;
    }

    public void Teleport(object sender, GridPosition gridPosition)
    {
        availableTeleports--;
        movement.ToggleController(false);
        transform.position = LevelGrid.Instance.GetCellCentre(gridPosition);
        movement.ToggleController(true);
        movement.ToggleMovement(true);
    }
}
