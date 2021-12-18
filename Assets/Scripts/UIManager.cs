using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

   [SerializeField] private Dropdown runningModeDropdown;
   [SerializeField] private Dropdown selectAlgoDropdown;
   [SerializeField] private Button startButton;
   [SerializeField] private Text startButtonText;
   [SerializeField] private Text exploredTilesText;
   [SerializeField] private Text timeText;
   [SerializeField] private GameObject warningText;

    private Toggle currentToggle;

    private void Awake() 
    {
        instance = this;
        warningText.SetActive(false);
    }

    private void Start() 
    {
        startButton.onClick.AddListener(OnPressStartButton);
    }

    public void StartToggleChange(Toggle toggle)
    {
        if(currentToggle != null && currentToggle != toggle) currentToggle.isOn = false;
        currentToggle = toggle;

        if(currentToggle.isOn)
        GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.start;
        else GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.none;
    }

    public void TargetToggleChange(Toggle toggle)
    {
        if(currentToggle != null && currentToggle != toggle) currentToggle.isOn = false;
        currentToggle = toggle;

        if(currentToggle.isOn)
        GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.target;
        else GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.none;
    }

    public void EmptyToggleChange(Toggle toggle)
    {
        if(currentToggle != null && currentToggle != toggle) currentToggle.isOn = false;
        currentToggle = toggle;

        if(currentToggle.isOn)
        GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.empty;
        else GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.none;
    }

    public void BlockToggleChange(Toggle toggle)
    {
        
        if(currentToggle != null && currentToggle != toggle) currentToggle.isOn = false;
        currentToggle = toggle;

        if(currentToggle.isOn)
        GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.block;
        else GameplayManager.instance.CurrentTileSelection = GameplayManager.TileSelection.none;
    }

    public void OnPressStartButton()
    {
        if(runningModeDropdown.value == 0)
        {
            if(selectAlgoDropdown.value == 0)   
            GameplayManager.instance.StartMovement(PathfindingManager.Algorithms.Astar);

            else GameplayManager.instance.StartMovement(PathfindingManager.Algorithms.Dijkstra);
        }

        //Show Calculation process only.
        else
        {
            if(selectAlgoDropdown.value == 0)   
            GameplayManager.instance.ShowCalculationAlgorithm(PathfindingManager.Algorithms.Astar);

            else GameplayManager.instance.ShowCalculationAlgorithm(PathfindingManager.Algorithms.Dijkstra);
        }

        startButtonText.text = "Reset";
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnPressResetButton);
    }

    public void OnPressResetButton()
    {
        warningText.SetActive(false);
        GameplayManager.instance.StopCalculationCoroutines();
        startButtonText.text = "Start";
        Agent.instance.ResetAgentPositionAndMovement();
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnPressStartButton);
        TileManager.instance.ResetTileStats();   
    }

    public void  UpdateTimeText(float val)
    {
        timeText.text = "Time Process(x10) = " + val.ToString();
    }

    public void UpdateTilesCount(int val)
    {
        exploredTilesText.text = "Explored Tiles = " + val.ToString();
    }

    public void MakeWarning()
    {
        warningText.SetActive(true);
    }
}
