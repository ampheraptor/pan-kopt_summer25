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
        PlaceTraverser();
        mTraverser.SetStepSize(cellWidth);

    }

    private ProtoMSCell RandomNoNeighborCell()
    {
        //Copied functionality of RandomEmpty CEll
        ProtoMSCell pms = grid[Random.Range(0, ROWS), Random.Range(0, COLS)];
        if (pms.tileData.GetType() != typeof(NumberTile))
        {
            return RandomNoNeighborCell();
            
        }
        else
        {
            NumberTile nt = (NumberTile)pms.tileData;
            if (nt.GetMine() > 0 || nt.GetNeighorMineCount() > 0){
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
        emptyCell.tileData.TryReveal();
        
    }
}
