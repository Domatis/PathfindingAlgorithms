using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    

    public static PathfindingManager instance;

    private int flatcost = 10;
    private int diagonalCost = 14;

    private void Awake() 
    {
        instance = this;
    }

    public List<Tile> AStarAlgorithmPathfinding(Tile startTile, Tile targetTile)
    {

        float starttimeVal  = Time.time;

        List<Tile> path = new List<Tile>();
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(startTile);
        Tile currentTile = startTile;


        while(openList.Count > 0)
        {
            //Check the current tile first is it targettile or not.
            if(currentTile == targetTile)
            {
                //Find the parents of the targettile to start tile and add to tiles to path list and return it.

                while(currentTile.ParentTile != null)
                {
                    path.Add(currentTile);
                    currentTile = currentTile.ParentTile;
                }
                path.Reverse();
                
                return path;
            }

            else
            {   
                //Remove current tile from open list and add it to closed list.
                openList.Remove(currentTile);
                closedList.Add(currentTile);

                //Find the successors of current tile. 
                for(int i =0; i < 8;i++)    //Max number of successor count.
                {
                    CalculateSuccessorIndexValues(i,currentTile.XPos,currentTile.YPos,out int successorColumnVal, out int successorRowVal);
             
                    Tile successor = TileManager.instance.GetTileByIndex(successorColumnVal,successorRowVal);
                    //Check the successors tiles that they'r blocked if they blocked just skip.
                    if(successor == null) continue; 
                    if(successor.IsTileBlocked() || closedList.Contains(successor)) continue;
                    //Calculate successor current fcost, 
                    int currentSuccessorGCost = currentTile.GCost + CalculateDistanceCostBetWeenTiles(currentTile,successor);
                    int currentSuccessorHCost = CalculateDistanceCostBetWeenTiles(successor,targetTile);
                    int currentSuccessorFcost = currentSuccessorGCost+currentSuccessorHCost;

                    if(currentSuccessorFcost < successor.FCost)
                    {  
                        //if it's current fcost higher than current one update it's cost values and parent tile.
                        successor.GCost =  currentSuccessorGCost;
                        successor.HCost = currentSuccessorHCost;
                        successor.ParentTile = currentTile;
                    }   

                    //Check wheter they'r at open list or not add them to the open list if necessary.
                    if(!openList.Contains(successor)) openList.Add(successor);
                }

                //At the end check the openlist and find the lowest fcost tile and set the currenttile to it.

                int nextTileIndex = 0 ;// For reference

                for(int i= 0; i < openList.Count;i++)
                {
                    if(openList[i].FCost < openList[nextTileIndex].FCost)
                    {
                        nextTileIndex = i;
                    }
                }
                currentTile = openList[nextTileIndex];

            }
        }

        //If could'nt find and open list and just return null.
        return null;
    }

    private void CalculateSuccessorIndexValues(int indexNum, int currentColumnVal, int currentRowVal, out int resultcolumnVal, out int resultRowVal) 
    {
        resultcolumnVal = 0;
        resultRowVal = 0;

        switch(indexNum)
        {
            case 0: //North
            resultcolumnVal = currentColumnVal+1;
            resultRowVal = currentRowVal;
            break;
            case 1: //North-East
            resultcolumnVal = currentColumnVal+1;
            resultRowVal = currentRowVal +1;
            break;
            case 2: //East 
            resultcolumnVal = currentColumnVal;
            resultRowVal = currentRowVal +1;
            break;
            case 3: //South-East
            resultcolumnVal = currentColumnVal-1;
            resultRowVal = currentRowVal +1;
            break;
            case 4: //South
            resultcolumnVal = currentColumnVal-1;
            resultRowVal = currentRowVal;
            break;
            case 5: //South-West
            resultcolumnVal = currentColumnVal-1;
            resultRowVal = currentRowVal -1;
            break;
            case 6: //West
            resultcolumnVal = currentColumnVal;
            resultRowVal = currentRowVal -1;
            break;
            case 7: //North-West
            resultcolumnVal = currentColumnVal+1;
            resultRowVal = currentRowVal -1;
            break;
        }
        
    }




    private int CalculateDistanceCostBetWeenTiles(Tile currentTile,Tile targetTile)
    {
        int xDist = Mathf.Abs(currentTile.YPos - targetTile.YPos);
        int yDist = Mathf.Abs(currentTile.XPos - targetTile.XPos);

        return (Mathf.Min(xDist,yDist) * diagonalCost) + (Mathf.Abs(xDist-yDist) * flatcost);
    }



}
