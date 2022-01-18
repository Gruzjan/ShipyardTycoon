using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Player player = new Player {name = "siema" }; //create current player

    [SerializeField]
    private Slider balanceSlider;
    [SerializeField]
    private Text balanceText;
    [SerializeField]
    private Text premiumBalanceText;
    [SerializeField]
    private Slider lvlBar;

    private Shipyard currentShipyard;

    void Awake()
    {
        player.shipyardsOwned.Add(new Shipyard { timeToBuild = 5f }); //5 seconds to build //first player's shipyard
        currentShipyard = player.shipyardsOwned[int.Parse(SceneManager.GetActiveScene().name)];
    }

    void Start()
    {
        balanceText.text = balanceSlider.value.ToString();
        premiumBalanceText.text = player.premiumBalance.ToString();
        addMoney(100);
    }

    public void addExp(int Exp)
    {
        if (lvlBar.value + Exp >= lvlBar.maxValue)
        {
            currentShipyard.lvl++;
            lvlBar.transform.GetChild(2).GetComponent<Text>().text = currentShipyard.lvl.ToString();
            lvlBar.value = lvlBar.minValue;
        }
        else
            lvlBar.value += Exp;

        currentShipyard.lvlProgress = lvlBar.value;
    }

    public void addMoney(int money)
    {
        player.balance += money;
        StopCoroutine("ManageMoney");
        StartCoroutine("ManageMoney");
    }

    IEnumerator ManageMoney()
    {
        float speed = player.balance > 10000 ? 1600 : 200;
        while (balanceSlider.value != player.balance)
        {
            if (balanceSlider.value < player.balance)
            {
                balanceSlider.value += speed * Time.deltaTime;
                balanceText.text = balanceSlider.value.ToString();
            }
            else if (balanceSlider.value > player.balance)
            {
                balanceSlider.value = player.balance;
                balanceText.text = player.balance.ToString();
            }
            yield return null;
        }
    }

}
