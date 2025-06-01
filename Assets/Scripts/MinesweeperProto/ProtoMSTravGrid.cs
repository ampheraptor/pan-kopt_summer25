using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMSTravGrid : ProtoMSGrid
{
    [SerializeField] private GameObject mTraverserPrefab;

    protected void Initalize()
    {
        Initalize();
        PlaceTraverser();

    }

    private ProtoMSCell RandomNoNeighborCell()
    {
        //Copied functionality of RandomEmpty CEll
        ProtoMSCell pms = grid[Random.Range(0, ROWS), Random.Range(0, COLS)];
        if (pms.GetMine() || pms.GetNeighborMineCount() > 0)
        {
            return RandomNoNeighborCell();
        }

        return pms;

    }

    private void PlaceTraverser()
    {
        ProtoMSCell emptyCell = RandomNoNeighborCell();
        GameObject trav = Instantiate(mTraverserPrefab);
        trav.transform.position = emptyCell.transform.position;
        emptyCell.RevealRecursive();
    }
}
