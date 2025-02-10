using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IHM : MonoBehaviour
{
    [SerializeField]
    private TMP_Text cheerPlayer;

    [SerializeField]
    private TMP_Text waterAmount;

    [SerializeField]
    private TMP_Text proteinAmount;

    [SerializeField]
    private TMP_Text personalGrowthAmount;

    [SerializeField]
    private TMP_Text lifeAmount;

    public GameManager gameManager;

    void Start()
    {
        DisplayData();
    }


    void Update()
    {

    }

    void DisplayData()
    {
        cheerPlayer.text = $"GO GO GO\n{gameManager.currentPlayer.playersName.ToUpper()}\n! ! !";
        waterAmount.text = gameManager.currentPlayer.water.ToString();
        proteinAmount.text = gameManager.currentPlayer.protein.ToString();
        personalGrowthAmount.text = gameManager.currentPlayer.personalGrowth.ToString();
        lifeAmount.text = gameManager.currentPlayer.life.ToString();
    }
}