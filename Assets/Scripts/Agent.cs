using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    

    public static Agent instance;

    [SerializeField] private float agentSpeed;


    public Action movementEndEvent;
    public Action movementStartEvent;

    private bool moving;

    private List<Tile> currentPath;
    private int pathIndex = 0;
    private Vector3 dirToTarget;
    private Vector3 lastStartPosition;


    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
        lastStartPosition = transform.position;
    }

    private void Update() 
    {
        if(moving)
        {
            if(currentPath != null)
            {
                
                transform.position += dirToTarget * agentSpeed * Time.deltaTime;

                Vector3 distanceToTarget = currentPath[pathIndex].transform.position - transform.position;
                Debug.Log("distancetoTarget = " + distanceToTarget.sqrMagnitude);
                if(distanceToTarget.sqrMagnitude < 0.01f)
                {
                    //Update the next target.
                    pathIndex++;
                    if(pathIndex >= currentPath.Count)
                    {
                        moving = false;
                        transform.position = currentPath[pathIndex-1].transform.position;
                        movementEndEvent?.Invoke();
                        return;
                    }

                    //Update new direction
                    Vector3 newdistanceToTarget = currentPath[pathIndex].transform.position - transform.position;
                    dirToTarget = newdistanceToTarget.normalized;
                    Debug.Log("New Dir = " + dirToTarget);
                }

            }
        }
    }

    public void StartMovement(List<Tile> path)
    {  
        movementStartEvent?.Invoke();     
        currentPath = path;
        moving = true;
        pathIndex = 0;
        Debug.Log("Path Count = " + path.Count);

        Vector3 distanceToTarget = currentPath[pathIndex].transform.position - transform.position;
        dirToTarget = distanceToTarget.normalized;
    }
    
    public void SetPosition(Vector3 pos)
    {   
        lastStartPosition = pos;
        transform.position  = lastStartPosition;
    }

    public void ResetAgentPositionAndMovement()
    {
        moving = false;
        movementEndEvent?.Invoke();
        transform.position = lastStartPosition;

    }
}
