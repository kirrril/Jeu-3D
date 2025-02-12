using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IHM : MonoBehaviour
{
    public static IHM instance;

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

    [SerializeField]
    private TMP_Text speedAmount;

    GameManager gameManager {get {return GameManager.instance;}}

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DisplayData();
    }


    void Update()
    {

    }

    public void DisplayData()
    {

        cheerPlayer.text = $"GO GO GO\n{gameManager.currentPlayer.playersName.ToUpper()}\n! ! !";
        waterAmount.text = gameManager.currentPlayer.water.ToString();
        proteinAmount.text = gameManager.currentPlayer.protein.ToString();
        personalGrowthAmount.text = gameManager.currentPlayer.personalGrowth.ToString();
        lifeAmount.text = gameManager.currentPlayer.life.ToString();
        speedAmount.text = gameManager.currentPlayer.speed.ToString();

    }
}