using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public static TileManager instance;
    
    [SerializeField] private int columnValue;
    [SerializeField] private int rowValue;
    [SerializeField] private float distanceX = 1.15f;
    [SerializeField] private float distanceY = 1.05f;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private GameObject tilePrefab;

    private List<Tile> tiles = new List<Tile>();

    
    public List<Tile> Tiles {get {return tiles;}}

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
            GenerateTileArea();
    }

    public void GenerateTileArea()
    {
        //As default we generate the tiles from left-bottom corner.
        for(int i =0; i < rowValue;i++)
        {       
            for(int j=0; j <columnValue;j++)
            {
                Vector3 spawnPoint = startingPoint.transform.position + new Vector3(j*distanceX,i*distanceY,0);
                GameObject tileGameobject = Instantiate(tilePrefab,spawnPoint,Quaternion.identity);
                Tile tile = tileGameobject.GetComponent<Tile>();
                tile.SetTilePositions(j,i);
                tiles.Add(tile);
            }
        }
    }

    public Tile GetTileByIndex(int columnIndex,int rowIndex)
    {
        Tile result = null;

        for(int i =0; i < tiles.Count;i++)
        {
            if(tiles[i].XPos == columnIndex && tiles[i].YPos == rowIndex)
            {
                result = tiles[i];
                break;
            }
        }
        return result;
    }

    public Vector3 GetTilePositionByIndex(int columnIndex, int rowIndex)
    {
        Vector3 result = Vector3.zero;

        for(int i =0; i < tiles.Count;i++)
        {
            if(tiles[i].XPos == columnIndex && tiles[i].YPos == rowIndex)
            {
                result = tiles[i].transform.position;
                break;
            }
        }

        return result;
    }

    public void ResetTileStats()
    {
        for(int i=0; i < Tiles.Count;i++)
        {
            Tiles[i].GCost = 0;
            Tiles[i].HCost = int.MaxValue;
            Tiles[i].ParentTile = null;
            Tiles[i].ResetVisualSprites();

        }
    }
    
}
