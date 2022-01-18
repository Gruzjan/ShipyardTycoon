using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PanelsManager : MonoBehaviour
{
    [SerializeField]
    private WorkersManager workersManager;
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private List<GameObject> panels = new List<GameObject>(); //what to hide
    [SerializeField]
    private Slider shipProgressBar;
    [SerializeField]
    private GameObject premiumShopPanel;
    [SerializeField]
    private GameObject premiumShopButton;
    [SerializeField]
    private GameObject normalShopButton;
    [SerializeField]
    private GameObject confirmWorkerPanel;
    [SerializeField]
    private GameObject hirePanel;

    private Shipyard currentShipyard;

    void Awake()
    {
        currentShipyard = gameManager.player.shipyardsOwned[int.Parse(SceneManager.GetActiveScene().name)];
    }

    public void SwitchPanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);

        if(panel.activeSelf)
            for (int i = 0; i < panels.Count; i++)
                if(panels[i] != panel)
                    panels[i].SetActive(false);
        confirmWorkerPanel.SetActive(false);
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        for (int i = 0; i < panels.Count; i++)
            panels[i].SetActive(false);
        confirmWorkerPanel.SetActive(false);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ManageProgressBars()
    {
        //show/hide progress bars depending on if hire panel is active
        if (!hirePanel.activeSelf && shipProgressBar.IsActive())
            for (int i = 0; i < currentShipyard.workers.Count; i++)
                currentShipyard.workers[i].progressBar.SetActive(true);
        else if (hirePanel.activeSelf || !shipProgressBar.IsActive())
            for (int i = 0; i < currentShipyard.workers.Count; i++)
                currentShipyard.workers[i].progressBar.SetActive(false);
    }
    

    public void ConfirmWorker(string spawnId)
    {
        confirmWorkerPanel.SetActive(false); //opening panel animation
        int cost = currentShipyard.workersPrice[currentShipyard.workers.Count];
        GameObject buyBtn = confirmWorkerPanel.transform.GetChild(5).gameObject;
        buyBtn.GetComponentInChildren<Text>().text = cost.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
        buyBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        Debug.Log("spawnId confirm worker: " + spawnId);
        buyBtn.GetComponent<Button>().onClick.AddListener(delegate { workersManager.HireWorker(spawnId); });
        confirmWorkerPanel.SetActive(true);
    }


    public void switchHireLock()
    {
        //todo: some nice lock
    }

    public void SwitchShop(GameObject panelOfButton) //panelOfButton is panel of button pressed -> if normal shop button pressed, panelOfButton = normal shop panel //normal shop panel is always active we only toggle premium shop panel
    {
        //if premium shop button is pressed
        if (premiumShopPanel == panelOfButton)
            premiumShopPanel.SetActive(true);
        else
            premiumShopPanel.SetActive(false);
    }

}
