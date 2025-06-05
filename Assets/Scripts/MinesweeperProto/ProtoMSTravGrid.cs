using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMSTravGrid : ProtoMSGrid
{
    [SerializeField] private GameObject mTraverserPrefab;

    private ProtoTraverser mTraverser;

    public override void StartGrid()
    {
        base.StartGrid();
        InitTravGrid();
    }

    private void InitTravGrid()
    {
        PlaceTraverser();
        mTraverser.SetStepSize(cellWidth);
    }

    public override void RestartGrid()
    {
        base.RestartGrid();
        Destroy(mTraverser.gameObject);
        InitTravGrid();
    }

    
    public override void ReportMineTriggered(Item triggeredBy = null)
    {
        if (mTraverser.GetShield())
        {
            mTraverser.RestoreFuelFromMine();
        }
        else
        {
            base.ReportMineTriggered(triggeredBy);
        }
    }
    
    private ProtoMSCell RandomNoNeighborCell()
    {
        //Copied functionality of RandomEmpty CEll
        ProtoMSCell pms = grid[Random.Range(0, ROWS), Random.Range(0, COLS)];
        if (pms.tileData.GetType() != typeof(NumberTile)) // If this isn't a numberTile
        {
            return RandomNoNeighborCell();
            
        }
        else
        {
            NumberTile nt = (NumberTile)pms.tileData;
            if (nt.GetMine() > 0 || nt.GetNeighorMineCount() > 0){ //if it is make sure it has no mines and neigjbor count is 0
                return RandomNoNeighborCell();
            }
            
        }


        return pms;

    }

    private void PlaceTraverser()
    {
        ProtoMSCell emptyCell = RandomNoNeighborCell();
        GameObject trav = Instantiate(mTraverserPrefab);
        trav.transform.position = emptyCell.transform.position;
        mTraverser = trav.GetComponent<ProtoTraverser>();
        mTraverser.SetCurrentCell(emptyCell);
        emptyCell.tileData.TryReveal();
        
    }
}
