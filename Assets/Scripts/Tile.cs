using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    

    [SerializeField] private GameObject blockSpriteObj;
    [SerializeField] private GameObject closedSpriteObj;
    [SerializeField] private GameObject openSpriteObj;
    [SerializeField] private GameObject currentSpriteObj;

    private int xPos;
    private int yPos;
    

    private int gcost;    //Total cost to the from the starting point the genareted path the this tile.  
    private int hcost;    //Heuristic cost the total cost to target tile.
    private bool tileBlocked = false;
    private bool tileisTarget = false;
    private bool tileisStart = false;
    private Tile parentTile = null;
    
    public int XPos {get{return xPos;}}
    public int YPos {get{return yPos;}}

    public int FCost { get{return gcost + hcost;} }
    public int GCost { get{return gcost;} set {gcost = value;} }
    public int HCost { get{return hcost; } set{hcost = value;} }

    public bool TileisTarget {get {return tileisTarget;} set { tileisTarget = value;}}
    public bool TileisStart { get {return tileisStart;} set{tileisStart = value;}}

    public Tile ParentTile { get{return parentTile;} set{ parentTile = value;}}

    private void Start() 
    {
        gcost = 0;
        hcost = int.MaxValue;
    }

    public void SetTilePositions(int xVal, int yVal)
    {
        this.xPos = xVal;
        this.yPos = yVal;
    }

    public void SetTileBlock(bool state)
    {
        tileBlocked = state;
        blockSpriteObj.SetActive(tileBlocked);
    }

    public void SetTileOpen(bool state)
    {
        openSpriteObj.SetActive(state);
        closedSpriteObj.SetActive(!state);
        currentSpriteObj.SetActive(!state);
    }

    public void SetTileClosed(bool state)
    {
        closedSpriteObj.SetActive(state);
        openSpriteObj.SetActive(!state);
        currentSpriteObj.SetActive(!state);
    }

    public void SetTileCurrent(bool state)
    {
        currentSpriteObj.SetActive(state);
        closedSpriteObj.SetActive(!state);
        openSpriteObj.SetActive(!state);
    }

    public void ResetVisualSprites()
    {
        openSpriteObj.SetActive(false);
        closedSpriteObj.SetActive(false);
        currentSpriteObj.SetActive(false);
    }

    public bool IsTileBlocked()
    {
        return tileBlocked;
    }

    public void UpdateValues(Tile parenttile,int gcost,int hcost)
    {
        this.gcost = gcost;
        this.hcost = hcost;
        this.parentTile = parenttile;

    }
}
