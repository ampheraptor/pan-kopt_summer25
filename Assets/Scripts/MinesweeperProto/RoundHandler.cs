using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum roundState
{
    InProgress,
    Won,
    Died
}

public class RoundHandler : Singleton<RoundHandler>
{
    public roundState currentState;
    [SerializeField] private ProtoMSGrid grid;
    [SerializeField] private GameObject deathPopup;
    [SerializeField] private GameObject victoryPopup;

    private void Start()
    {
        
        grid.StartGrid();
    }

    public void ReportMineTriggered(Item triggeredBy=null) //any mine can tell the round handler that it was detonated, RH decides what to do next
    {
        if(triggeredBy && triggeredBy.overrideFailure)
        {
            Debug.Log("item " + triggeredBy.gameObject.name + " override the failstate it triggered");
        }
        else
        {
            TriggerDeath();

        }
    }

    public void ReportVictory()
    {
        TriggerVictory();
    }

    private void TriggerVictory()
    {
        if(currentState == roundState.InProgress)
        {
            currentState = roundState.Won;
            victoryPopup.SetActive(true);
        }
    }

    private void TriggerDeath()
    {
        if(currentState == roundState.InProgress)
        {
            currentState = roundState.Died;
            //this is where i'd put additional UI info or whatever in the popup
            deathPopup.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        if(currentState != roundState.InProgress)
        {
            currentState = roundState.InProgress;
            deathPopup.SetActive(false);
            victoryPopup.SetActive(false);
            grid.RestartGrid();
        }
    }

    public ProtoMSGrid GetGrid()
    {
        return grid;
    }
}
