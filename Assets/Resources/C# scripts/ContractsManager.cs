using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ContractsManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private ProgressBarsManager progressBarManager;
    [SerializeField]
    private WorkersManager workersManager;

    private Shipyard currentShipyard;

    [SerializeField]
    private Text[] prizeTexts = new Text[4]; //where to generate prizes 
    [SerializeField]
    private Text[] healthTexts = new Text[4]; //where to generate healths
    [SerializeField]
    private Ship[] ships = new Ship[4]; //ships in contracts
    [SerializeField]
    private GameObject contractPanel;
    [SerializeField]
    private GameObject lockPanel;
    [SerializeField]
    private Text lvlText;
    [SerializeField]
    private Slider shipProgressBar;
    [SerializeField]
    private Text shipValueText; 


    void Awake()
    {
        currentShipyard = gameManager.player.shipyardsOwned[int.Parse(SceneManager.GetActiveScene().name)];
    }

    void Start()
    {
        lvlText.text = currentShipyard.lvl.ToString();
        CreateContracts();  // todo: check if there are already created contracts
    }

    public void StartContract(int buttonId)
    {
        if (!shipProgressBar.IsActive())
        {
            ships[0] = ships[buttonId]; //make chosen ship the first element so you can end contract easily (you dont have to pass buttonId anywhere again)
            shipValueText.text = ships[0].value.ToString();
            shipProgressBar.maxValue = ships[0].health;
            progressBarManager.progressText.text = "0/" + shipProgressBar.maxValue.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
            contractPanel.SetActive(false);
            SwitchProgressBar(); //show progress bar
            StartCoroutine(progressBarManager.ProgressShip());
            workersManager.CheckWorkersStatus();
            InvokeRepeating("IncrementWorkers", 0, 0.05f);
            StartCoroutine(progressBarManager.ProgressWorkers());
        }
    }

    public void EndContract()
    {
        if (shipProgressBar.IsActive())
        {
            StopCoroutine(progressBarManager.ProgressWorkers());
            StopCoroutine(progressBarManager.ProgressShip());
            SwitchProgressBar(); //hide progress bar
            CancelInvoke("IncrementWorkers");
            workersManager.CheckWorkersStatus();
            shipProgressBar.value = shipProgressBar.minValue; //reset ship progress bar

            CreateContracts(); //create new set of contracts

            //disable lock panel
            if (contractPanel.activeSelf && lockPanel.activeSelf)
                lockPanel.SetActive(false);

            currentShipyard.safeBalance += ships[0].value; //todo: shop item safe capacity
            gameManager.addExp(ships[0].value);

            //reset workers' progress bars
            for (int i = 0; i < currentShipyard.workers.Count; i++)
                currentShipyard.workers[i].progressBar.GetComponent<Slider>().value = 0;
        }
    }

    public void IncrementWorkers()
    {
        for (int i = 0; i < currentShipyard.workers.Count; i++)
            currentShipyard.workers[i].targetProgress = currentShipyard.workers[i].progressBar.GetComponent<Slider>().value + currentShipyard.workers[i].workForce[currentShipyard.workers[i].lvl - 1] * 0.05f;
            //currentShipyard.workers[i].targetProgress = currentShipyard.workers[i].progressBar.GetComponent<Slider>().value + 2 * 0.1f;
    }

    public void SwitchProgressBar()
    {
        shipProgressBar.gameObject.active = !shipProgressBar.gameObject.active;
    }

    public void CheckContractsLock()
    {
        if (shipProgressBar.IsActive())
            lockPanel.SetActive(true);
        else if (!shipProgressBar.IsActive())
            lockPanel.SetActive(false);
    }

    public void CreateContracts()
    {
        for (int i = 0; i < 4; i++)
        {
            ships[i] = CreateShip();
            prizeTexts[i].text = ships[i].value.ToString();
            healthTexts[i].text = ships[i].health.ToString();
        }
    }

    public void ClaimSafeMoney()
    {
        gameManager.addMoney(currentShipyard.safeBalance);
        currentShipyard.safeBalance = 0;
    }

    public Ship CreateShip()
    {
        int shipyardLvl = currentShipyard.lvl - 1;
        int shipValue;
        //Im so sorry for this :(( TODO: DO IT BETTER
        if (shipyardLvl < 3){
            shipValue = (shipyardLvl + 1) * 10;
            return new Ship { skinType = 0, value = shipValue, health = shipValue}; //TODO: skin id is random
        }else if(shipyardLvl < 5){
            shipValue = Random.Range(40, 60);
            return new Ship { skinType = 1, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 8){
            shipValue = Random.Range(100, 250);
            return new Ship { skinType = 2, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 10){
            shipValue = Random.Range(300, 400);
            return new Ship { skinType = 2, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 12){
            shipValue = Random.Range(400, 500);
            return new Ship { skinType = 3, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 14){
            shipValue = Random.Range(550, 650);
            return new Ship { skinType = 3, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 16){
            shipValue = Random.Range(700, 900);
            return new Ship { skinType = 3, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 18){
            shipValue = Random.Range(1000, 1500);
            return new Ship { skinType = 4, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 20){
            shipValue = Random.Range(1400, 1900);
            return new Ship { skinType = 4, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 22){
            shipValue = Random.Range(2000, 3500);
            return new Ship { skinType = 4, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 24){
            shipValue = Random.Range(3500, 5000);
            return new Ship { skinType = 4, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 27){
            shipValue = Random.Range(5500, 7500);
            return new Ship { skinType = 5, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 30){
            shipValue = Random.Range(7000, 10000);
            return new Ship { skinType = 5, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 32){
            shipValue = Random.Range(10000, 14000);
            return new Ship { skinType = 6, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 34){
            shipValue = Random.Range(14500, 19000);
            return new Ship { skinType = 6, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 36){
            shipValue = Random.Range(19000, 25000);
            return new Ship { skinType = 6, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 38){
            shipValue = Random.Range(25000, 30000);
            return new Ship { skinType = 6, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 40){
            shipValue = Random.Range(30000, 40000);
            return new Ship { skinType = 7, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 43){
            shipValue = Random.Range(45000, 65000);
            return new Ship { skinType = 7, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 45){
            shipValue = Random.Range(75000, 90000);
            return new Ship { skinType = 8, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 46){
            shipValue = Random.Range(95000, 110000);
            return new Ship { skinType = 9, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 48){
            shipValue = Random.Range(110000, 130000);
            return new Ship { skinType = 9, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }else if (shipyardLvl < 50){
            shipValue = Random.Range(150000, 250000);
            return new Ship { skinType = 10, value = shipValue, health = Mathf.RoundToInt(shipValue * Random.Range(0.9f, 0.99f)) };
        }
        return new Ship { };
    }
}
