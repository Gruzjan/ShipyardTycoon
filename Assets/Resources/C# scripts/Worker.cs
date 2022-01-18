using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Worker
{
    public int lvl = 1;
    public int skinId = 0;
    public int id;
    public float targetProgress = 0;
    public int[] workForce = new int[10] { 25, 40, 50, 70, 90, 100, 125, 150, 175, 200 };
    public int[] lvlCost = new int[10] { 0, 100, 250, 1000, 2500, 5000, 10000, 15000, 25000, 50000};
    public GameObject lvlBar;
    public GameObject progressBar;
}
