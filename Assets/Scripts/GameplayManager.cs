using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    [SerializeField] private Agent startAgent;
    [SerializeField] private GameObject targetObject;

    private Tile lastStarttile = null;
    private Tile lastTargetTile = null;




    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
        //Default start and target positions.
        Tile startTile = TileManager.instance.Tiles[0];
        Tile targetTile = TileManager.instance.Tiles[TileManager.instance.Tiles.Count-1];
        
        SetAgentPositionToTile(startTile);
        SetTargetPositionToTile(targetTile);

    }
 


    public void StartMovement(PathfindingManager.Algorithms algoType)
    {
        List<Tile> path = null;

        if(algoType == PathfindingManager.Algorithms.Astar)
        {
            float startAstarTime = Time.realtimeSinceStartup;

            for(int i =0; i < 1000;i++)
            {
                path = PathfindingManager.instance.AStarAlgorithmPathfinding(lastStarttile,lastTargetTile);
            }

            float endAstarTime = Time.realtimeSinceStartup;

            Debug.Log("A*star Time = " + (endAstarTime - startAstarTime));
        }

        else
        {
            float startDijkstraTime = Time.realtimeSinceStartup;


            for(int i=0; i < 1000;i++)
            {
            path = PathfindingManager.instance.DijkstraAlgorithmPathfinding(lastStarttile,lastTargetTile);
            }
        
            float endDijkstraTime = Time.realtimeSinceStartup;

            Debug.Log("Dijkstra Time = " + (endDijkstraTime - startDijkstraTime));
            startAgent.StartMovement(path);
        }
    }

    public void SetAgentPositionToTile(Tile tile)
    {
        startAgent.transform.position = tile.transform.position;
        tile.TileisStart = true;
        if(lastStarttile != null) lastStarttile.TileisStart = false;

        lastStarttile = tile;

    }

    public void SetTargetPositionToTile(Tile tile)
    {
        targetObject.transform.position = tile.transform.position;
        tile.TileisTarget = true;
        if(lastTargetTile != null) lastTargetTile.TileisTarget = false;

        lastTargetTile = tile;

    }



}
