using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProtoTravCanvas : Singleton<ProtoTravCanvas>
{
    [SerializeField] private TextMeshProUGUI fuelText;

   

    public TextMeshProUGUI GetFuelText()
    {
        return fuelText;
    }


}
