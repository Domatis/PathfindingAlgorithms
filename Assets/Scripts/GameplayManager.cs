using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public enum TileSelection {none,empty,block,start,target}

    public static GameplayManager instance;

    [SerializeField] private Agent startAgent;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float calculationSpeed;

    private TileSelection currentTileselection = TileSelection.none;
    private Tile lastStarttile = null;
    private Tile lastTargetTile = null;

    private WaitForSeconds calcDelay;
    private Coroutine lastStartedCoroutine;

    public TileSelection CurrentTileSelection {get{ return currentTileselection;} set{ currentTileselection = value;}}

    private int tileLayerMask;
    private bool mouseSelectionOn = false;
    private bool movementOn = false;

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {

        calcDelay = new WaitForSeconds(1/calculationSpeed);

        //Default start and target positions.
        Tile startTile = TileManager.instance.Tiles[0];
        Tile targetTile = TileManager.instance.Tiles[TileManager.instance.Tiles.Count-1];
        
        tileLayerMask = 1 << LayerMask.NameToLayer("Tiles");

        SetStartPositionToTile(startTile);
        SetTargetPositionToTile(targetTile);


        Agent.instance.movementStartEvent += () =>  {movementOn = true; mouseSelectionOn = false;};
        Agent.instance.movementEndEvent += () => {movementOn = false;};
    }

    private void Update() 
    {
        if(movementOn) return;  //Hareket halindeyken inputları alma.

        if(Input.GetMouseButtonDown(0))
        {
            mouseSelectionOn = true;
        }

        if(mouseSelectionOn)
        {
            //Ray gönder sadece tilelar için eğer var ise selected duruma göre işaretle.

            RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,10,tileLayerMask);
            

            if(raycastHit.collider != null)
            {
                Tile tile = raycastHit.collider.GetComponent<Tile>();

                switch(currentTileselection)
                {
                    case TileSelection.empty:
                    if(!tile.TileisStart && !tile.TileisTarget) tile.SetTileBlock(false);
                    break;
                    case TileSelection.block:
                    if(!tile.TileisStart && !tile.TileisTarget) tile.SetTileBlock(true);
                    break;
                    case TileSelection.start:
                    if(!tile.IsTileBlocked() && !tile.TileisTarget) GameplayManager.instance.SetStartPositionToTile(tile);
                    break;
                    case TileSelection.target:
                    if(!tile.IsTileBlocked() && !tile.TileisStart) GameplayManager.instance.SetTargetPositionToTile(tile);
                    lastTargetTile = tile;
                    break;
                }
            }


        }

        if(Input.GetMouseButtonUp(0))
        {
            mouseSelectionOn = false;
        }
    }


    public void StartMovement(PathfindingManager.Algorithms algoType)
    {
        List<Tile> path = null;

        if(algoType == PathfindingManager.Algorithms.Astar)
        {
            float startAstarTime = Time.realtimeSinceStartup;

            int exploredTilesCount = 0;

            for(int i =0; i < 10;i++)
            {
                path = PathfindingManager.instance.AStarAlgorithmPathfinding(lastStarttile,lastTargetTile,ref exploredTilesCount);
            }


            if(path != null)
            {
                float endAstarTime = Time.realtimeSinceStartup;
                UIManager.instance.UpdateTimeText(endAstarTime-startAstarTime);
                UIManager.instance.UpdateTilesCount(exploredTilesCount);
                startAgent.StartMovement(path);
            }
               
            else
            {
                UIManager.instance.MakeWarning();
            } 
          
        }

        else
        {
            float startDijkstraTime = Time.realtimeSinceStartup;

            int exploredTilesCount = 0;

            for(int i=0; i < 10;i++)
            {
                path = PathfindingManager.instance.DijkstraAlgorithmPathfinding(lastStarttile,lastTargetTile,ref exploredTilesCount);
            }
        
            if(path != null)
            {
                float endDijkstraTime = Time.realtimeSinceStartup;
                UIManager.instance.UpdateTimeText(endDijkstraTime-startDijkstraTime);
                UIManager.instance.UpdateTilesCount(exploredTilesCount);
                startAgent.StartMovement(path);
            }

            else
            {
                UIManager.instance.MakeWarning();
            }    
        }    
    }

    public void SetStartPositionToTile(Tile tile)
    {
        startAgent.SetPosition(tile.transform.position);
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

    public void ShowCalculationAlgorithm(PathfindingManager.Algorithms algoType)
    {
        if(algoType == PathfindingManager.Algorithms.Astar)
        lastStartedCoroutine =  StartCoroutine(PathfindingManager.instance.AStarShowCalculation(lastStarttile,lastTargetTile,calcDelay));
        else 
        lastStartedCoroutine = StartCoroutine(PathfindingManager.instance.DijkstraShowCalculation(lastStarttile,lastTargetTile,calcDelay));
    }

    public void StopCalculationCoroutines()
    {
        if(lastStartedCoroutine != null) StopCoroutine(lastStartedCoroutine);
    }


}
