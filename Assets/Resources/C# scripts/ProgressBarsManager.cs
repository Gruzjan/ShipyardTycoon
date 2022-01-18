using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBarsManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private ContractsManager contractsManager;

    private Shipyard currentShipyard;

    public Text progressText; // x/y
    [SerializeField]
    private ParticleSystem ParticleSystem;
    [SerializeField]
    private Text shipValueText;
    [SerializeField]
    private Slider shipProgressBar; //ship health progress bar
    [SerializeField]
    private float targetProgress = 0;

    public float defaultSpeed = 20f;

    void Awake()
    {
        currentShipyard = gameManager.player.shipyardsOwned[int.Parse(SceneManager.GetActiveScene().name)];
        Debug.Log("Scene: " + SceneManager.GetActiveScene().name);
    }

    public IEnumerator ProgressWorkers()
    {
        while (shipProgressBar.IsActive())
        {
            //if target progress is >= worker's progress bar's max value, increment progress, play particle, reset target progress and add part to ship health
            for (int i = 0; i < currentShipyard.workers.Count; i++)
            {
                Slider pb = currentShipyard.workers[i].progressBar.GetComponent<Slider>(); //worker's progress bar
                
                if (currentShipyard.workers[i].targetProgress >= pb.maxValue)
                {
                    //currentShipyard.workers[i].targetProgress = pb.maxValue - 0.01f;
                    pb.value += defaultSpeed * Time.deltaTime;
                    //show the little hammer animation
                    currentShipyard.workers[i].targetProgress = 0;
                    pb.value = pb.minValue;
                    targetProgress += 1; //TODO: Scale with worker's lvl, adjust the progress bars' max values too
                }
                //just increment to target progress
                else if (pb.value < currentShipyard.workers[i].targetProgress)
                {
                    pb.value += defaultSpeed * Time.deltaTime;
                }
            }
            yield return null;
        }
    }

    public IEnumerator ProgressShip()
    {
        while (shipProgressBar.IsActive())
        {
            // if target progress is >= ship's max health, increment progress, play particles, reset target progress and add Money
            if (shipProgressBar.value < targetProgress && targetProgress >= shipProgressBar.maxValue) //todo: idk about this
            {
                targetProgress = shipProgressBar.maxValue - 0.01f; // ??
                shipProgressBar.value += defaultSpeed * Time.deltaTime; //* int.Parse(shipValueText.text.Substring(10)) / 50; //TODO: recalculate
                progressText.text = shipProgressBar.value.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")) + "/" + shipProgressBar.maxValue.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
                if (!ParticleSystem.isPlaying)
                    ParticleSystem.Play();
                contractsManager.EndContract();
                targetProgress = 0;

            }
            // increment to target progress
            else if (shipProgressBar.value < targetProgress)
            {
                shipProgressBar.value += defaultSpeed * Time.deltaTime;// * int.Parse(shipValueText.text.Substring(10)) / 50; //TODO: Recalculate
                progressText.text = shipProgressBar.value.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")) + "/" + shipProgressBar.maxValue.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
            }
            yield return null;
        }
    }

    public void HurryWorkers()
    {
        if(shipProgressBar.IsActive())
            for (int i = 0; i < currentShipyard.workers.Count; i++)
                currentShipyard.workers[i].progressBar.GetComponent<Slider>().value += currentShipyard.workers[i].progressBar.GetComponent<Slider>().maxValue * gameManager.player.hurryWorkers;
    }

}
