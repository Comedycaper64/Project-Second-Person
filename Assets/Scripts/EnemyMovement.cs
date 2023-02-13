using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    private AIPath aIPath;

    private void Awake() 
    {
        aIPath = GetComponent<AIPath>();    
    }

    public void SetMovePosition(Vector3 movePosition)
    {
        aIPath.destination = movePosition;
    }
}
