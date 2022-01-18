using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string name = null;
    public int balance = 0;
    public int premiumBalance = 0;
    public int skinId = 0;
    public float hurryWorkers = 0.1f;
    public List<Shipyard> shipyardsOwned = new List<Shipyard>();
}
