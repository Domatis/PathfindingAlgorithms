using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private Agent startAgent;
    [SerializeField] private GameObject targetObject;


    public bool testBool = false;

    private void Update() 
    {
        if(testBool)
        {
            TestStartMovement();
            testBool = false;
        }
    }


    public void TestStartMovement()
    {
            List<Tile> path = null;

            float startTime = Time.realtimeSinceStartup;

        for(int i =0; i < 1; i++)
        {
             path = PathfindingManager.instance.AStarAlgorithmPathfinding(TileManager.instance.GetTileByIndex(0,0),TileManager.instance.GetTileByIndex(9,7));
        }

        Debug.Log(Time.realtimeSinceStartup - startTime);

        startAgent.transform.position = TileManager.instance.GetTilePositionByIndex(0,0);
        targetObject.transform.position = TileManager.instance.GetTilePositionByIndex(9,7);

        startAgent.StartMovement(path);
    }



}
