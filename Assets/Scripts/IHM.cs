using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IHM : MonoBehaviour
{
    public static IHM instance;

    [SerializeField]
    public Button stopTrainingButton;

    [SerializeField]
    public Image lifeImage;

    [SerializeField]
    public Image waterImage;

    [SerializeField]
    public Image proteinImage;

    [SerializeField]
    public Image legsImage;

    [SerializeField]
    public Image chestImage;

    [SerializeField]
    public Image backImage;



    public List<Sprite> remainingLives;

    public List<Sprite> proteinCollected;


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
        DisplayData();
    }

    public void DisplayData()
    {
        waterImage.fillAmount = GameManager.instance.currentPlayer.water;
        legsImage.fillAmount = GameManager.instance.currentPlayer.legsTraining;
        chestImage.fillAmount = GameManager.instance.currentPlayer.chestTraining;
        backImage.fillAmount = GameManager.instance.currentPlayer.backTraining;
        lifeImage.sprite = remainingLives[GameManager.instance.currentPlayer.life];
        proteinImage.sprite = proteinCollected[GameManager.instance.currentPlayer.protein];
    }
}