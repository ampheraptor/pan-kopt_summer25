using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMSTravGrid : ProtoMSGrid
{
    [SerializeField] private GameObject mTraverserPrefab;

    private void Start()
    {
        Initialize();
        PlaceTraverser();

    }

    private void PlaceTraverser()
    {

    }
}
