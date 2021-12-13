using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    
    [SerializeField] private float agentSpeed;



    private bool moving;

    private List<Tile> currentPath;
    private int pathIndex = 0;
    private Vector3 dirToTarget;

    private void Update() 
    {
        if(moving)
        {
            if(currentPath != null)
            {
                
                transform.position += dirToTarget * agentSpeed * Time.deltaTime;

                Vector3 distanceToTarget = currentPath[pathIndex].transform.position - transform.position;
                if(distanceToTarget.sqrMagnitude < 0.01f)
                {
                    transform.position = currentPath[pathIndex].transform.position;
                    //Update the next target.
                    pathIndex++;
                    if(pathIndex >= currentPath.Count)
                    {
                        moving = false;
                        Debug.Log("Movement stopped");
                        return;
                    }

                    //Update new direction
                    Vector3 newdistanceToTarget = currentPath[pathIndex].transform.position - transform.position;
                    dirToTarget = newdistanceToTarget.normalized;
                }

            }

        }
    }

    public void StartMovement(List<Tile> path)
    {
        moving = true;
        currentPath = path;
        pathIndex = 0;

        Vector3 distanceToTarget = currentPath[pathIndex].transform.position - transform.position;
        dirToTarget = distanceToTarget.normalized;
    }
    
    public void SetAgentToTile(Tile tile)
    {

    }
}
