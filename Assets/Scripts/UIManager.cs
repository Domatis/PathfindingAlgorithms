using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

   [SerializeField] private GameObject dropdownMenu;
   [SerializeField] private Dropdown selectAlgoDropdown;
   
    private Dropdown tileDropdown;
    

    private Tile currentSelectedTile;
    private bool dropdownActive= false;

    private float xOfsset = 60;
    private float yOffset = -15;

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {   
        tileDropdown = dropdownMenu.GetComponent<Dropdown>();
        dropdownMenu.SetActive(false);
    }

    public void ActiveDropdownMenu(Vector3 pos,Tile selectedtile)
    {
        if(currentSelectedTile == selectedtile) return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);

        screenPos.x += xOfsset;
        screenPos.y += yOffset;

        currentSelectedTile = selectedtile;


        dropdownMenu.SetActive(true);
        dropdownMenu.transform.position = screenPos;
        dropdownActive = true;    
    }

    public void DeActiveDropdownMenu()
    {
        
        currentSelectedTile = null;
        dropdownActive = false;
        tileDropdown.value = 0;  //Reset the value.
        dropdownMenu.SetActive(false);
    }

    public void TileDropdownChange()
    {
        switch(tileDropdown.value)
        {
            case 1:
            if(!currentSelectedTile.IsTileBlocked() && !currentSelectedTile.TileisTarget) GameplayManager.instance.SetAgentPositionToTile(currentSelectedTile);
            break;
            case 2:
            if(!currentSelectedTile.IsTileBlocked() && !currentSelectedTile.TileisStart) GameplayManager.instance.SetTargetPositionToTile(currentSelectedTile);
            break;
            case 3:
            if(!currentSelectedTile.TileisStart && !currentSelectedTile.TileisTarget) currentSelectedTile.SetTileBlock(false);
            break;
            case 4:
            if(!currentSelectedTile.TileisStart && !currentSelectedTile.TileisTarget) currentSelectedTile.SetTileBlock(true);
            break;
        }

        DeActiveDropdownMenu();
    }


    

    public void StartButton()
    {
        DeActiveDropdownMenu();
        if(selectAlgoDropdown.value == 0)   GameplayManager.instance.StartMovement(PathfindingManager.Algorithms.Astar);

        else GameplayManager.instance.StartMovement(PathfindingManager.Algorithms.Dijkstra);
        
    }

    


}
