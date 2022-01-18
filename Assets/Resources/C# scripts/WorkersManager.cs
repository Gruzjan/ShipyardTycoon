using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WorkersManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform position;
        public bool occupied = false;
    }

    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private SkinManager skinManager;

    private Shipyard currentShipyard;
    
    [SerializeField]
    private SpawnPoint[] spawnPoints = new SpawnPoint[8];
    [SerializeField]
    private Button[] hireButtons = new Button[8];
    [SerializeField]
    private GameObject shipProgressBar;
    [SerializeField]
    private GameObject hirePanel;
    [SerializeField]
    private GameObject workerLvlBarPrefab;
    [SerializeField]
    private GameObject workerProgressBarPrefab;
    [SerializeField]
    private GameObject confirmWorkerPanel;


    private void Awake()
    {
        currentShipyard = gameManager.player.shipyardsOwned[int.Parse(SceneManager.GetActiveScene().name)];
    }

    public void CheckWorkersStatus() //disable/enable hire panel components (hire button, lvl bar) on view change if(hire panel) //TODO: rethink this, espiecially the last to else if statements
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            if (hirePanel.activeSelf && !spawnPoints[i].occupied)
                hireButtons[i].gameObject.SetActive(true);
            else if (hirePanel.activeSelf && spawnPoints[i].occupied)
                for (int j = 0; j < currentShipyard.workers.Count; j++)
                {
                    if (currentShipyard.workers[j].id == i)
                    {
                        currentShipyard.workers[j].progressBar.SetActive(false);
                        currentShipyard.workers[j].lvlBar.SetActive(true);
                        break;
                    }
                }
            else if(!hirePanel.activeSelf && !spawnPoints[i].occupied)
                hireButtons[i].gameObject.SetActive(false);
            else if (!hirePanel.activeSelf && spawnPoints[i].occupied)
                for (int j = 0; j < currentShipyard.workers.Count; j++)
                {
                    if (currentShipyard.workers[j].id == i)
                    {
                        currentShipyard.workers[j].lvlBar.SetActive(false);
                        break;
                    }
                }
            else
                hireButtons[i].gameObject.SetActive(false);

            if (!hirePanel.activeSelf && shipProgressBar.activeSelf)
            {
                for (int j = 0; j < currentShipyard.workers.Count; j++)
                {
                    if (currentShipyard.workers[j].id == i)
                    {
                        currentShipyard.workers[j].progressBar.SetActive(true);
                        break;
                    }
                    
                }
            }
            else if (!hirePanel.activeSelf && !shipProgressBar.activeSelf)
            {
                for (int j = 0; j < currentShipyard.workers.Count; j++)
                {
                    if (currentShipyard.workers[j].id == i)
                    {
                        currentShipyard.workers[j].progressBar.SetActive(false);
                        break;
                    }
                }
            }

        }  
    }

    public void HireWorker(string spawnId)
    {
        int cost = currentShipyard.workersPrice[currentShipyard.workers.Count];
        if(gameManager.player.balance >= cost)
        {
            int skinId = skinManager.GetSelectedSkin();
            if (skinId < 10)
                spawnId += "0" + skinId.ToString();
            else
                spawnId += skinId.ToString();

            gameManager.addMoney(-cost);
            SpawnWorker(spawnId);
            confirmWorkerPanel.SetActive(false);
        }
    }

    public void SpawnWorker(string parameters) //example 102: -> 1 - spawnId; 02 -> skinId || spawnId range: 0-7, skindId range: 0-X TODO
    {
        int spawnId = int.Parse(parameters.Substring(0, 1));
        int skinId = int.Parse(parameters.Substring(1, 2));
        spawnPoints[spawnId].occupied = true;

        if(hireButtons[spawnId].IsActive())
            hireButtons[spawnId].gameObject.SetActive(false);

        Transform _sp = spawnPoints[spawnId].position;

        GameObject workerSpawned = new GameObject();
        workerSpawned.AddComponent<Image>();

        GameObject workerLvlBar = Instantiate(workerLvlBarPrefab);
        workerLvlBar.GetComponent<Button>().onClick.AddListener(delegate { LvlUpWorker(); });

        GameObject workerProgressBar = Instantiate(workerProgressBarPrefab);
        workerProgressBar.SetActive(false);

        if (spawnId < 4)
        {
            workerSpawned.GetComponent<Image>().sprite = skinManager.workerSkins[skinId].front;
            //workerSpawned.GetComponent<SpriteRenderer>().sortingOrder = 1;
            workerSpawned.transform.SetParent(spawnPoints[spawnId].position);
            workerSpawned.transform.localPosition = new Vector3(0, 20, 0);

            workerLvlBar.transform.SetParent(workerSpawned.transform);
            workerLvlBar.transform.localPosition = new Vector3(0, 85, 0);
            
            workerProgressBar.transform.SetParent(workerSpawned.transform);
            workerProgressBar.transform.localPosition = new Vector3(0, 65, 0);
        }
        else
        {
            workerSpawned.GetComponent<Image>().sprite = skinManager.workerSkins[skinId].back;
            //workerSpawned.GetComponent<SpriteRenderer>().sortingOrder = 1;
            workerSpawned.transform.SetParent(spawnPoints[spawnId].position);
            workerSpawned.transform.localPosition = new Vector3(0, 20, 0);

            workerLvlBar.transform.SetParent(workerSpawned.transform);
            workerLvlBar.transform.localPosition = new Vector3(0, -85, 0);

            workerProgressBar.transform.SetParent(workerSpawned.transform);
            workerProgressBar.transform.localPosition = new Vector3(0, -65, 0);
        }
        workerSpawned.transform.localScale = new Vector3(1.6f, 1.6f, 0);
        workerLvlBar.transform.GetChild(0).GetComponent<Text>().text = "$100";// currentShipyard.workers[0].lvlCost[1].ToString(); //TODO "100" IS A VARIABLE AND $ IS AN IMG

        currentShipyard.workers.Add(new Worker { skinId = skinId, id = spawnId, lvlBar = workerLvlBar, progressBar = workerProgressBar });
    }

    public void LvlUpWorker()
    {
        for (int i = 0; i < currentShipyard.workers.Count; i++)
            if (currentShipyard.workers[i].lvlBar == EventSystem.current.currentSelectedGameObject) //find pressed button by iterating hired workers list
            {
                int cost = currentShipyard.workers[i].lvlCost[currentShipyard.workers[i].lvl];
                if (gameManager.player.balance >= cost)
                {
                    LvlUp(currentShipyard.workers[i]);
                    gameManager.addMoney(-cost);
                }
                //else
                //fancy upgrade cost moving animation
                break;
            }
    }

    public void LvlUp(Worker worker)
    {
        if (worker.lvl < 9)
        {
            worker.lvlBar.transform.GetChild(1).transform.GetChild(worker.lvl).GetComponent<Image>().color = new Color32(255, 235, 28, 255); //make every lvl step yellow
            worker.lvl++;
            worker.lvlBar.transform.GetChild(0).GetComponent<Text>().text = "$" + worker.lvlCost[worker.lvl].ToString(); //TODO: $ IS AN IMG

        }
        else if (worker.lvl == 9)
        {
            worker.lvlBar.transform.GetComponent<Button>().interactable = false; //disable button interactability
            worker.lvl++;
            worker.lvlBar.transform.GetChild(0).gameObject.SetActive(false); //disable price text
            worker.lvlBar.transform.GetChild(1).gameObject.transform.GetChild(9).GetComponent<Image>().color = new Color32(255, 235, 28, 255); //make 10th step yellow
            worker.lvlBar.transform.GetComponent<Image>().color = new Color32(0, 122, 204, 255); //make button from yellow to blue
        }
    }

}
