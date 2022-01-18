using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Shipyard
{
    public string name = null;
    public int id = 0;
    public int lvl = 1;
    public float lvlProgress = 0;
    public float[] shipyardLvls = new float[50]; //how much money you need to lvl up shipyard //static //TODO
    public int[] workersPrice = new int[8] {100, 500, 3000, 10000, 30000, 50000, 80000, 150000} ; //how much money you need to pay for new worker //static //TODO
    public int safeBalance = 0;
    public float timeToBuild = 5f;
    public List<Worker> workers = new List<Worker>();
}
