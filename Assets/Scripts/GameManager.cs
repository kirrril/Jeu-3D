using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player currentPlayer;

    public float treadmillTraining;
    public float bikeTraining;
    public float jumpboxTraining;

    void Awake()
    {
        instance = this;

        LaunchGame();
    }

    void Update()
    {
        TrainingManagement();

        LifeManagement();
    }


    void LaunchGame()
    {
        currentPlayer = new Player("Kirill");
    }

    public void LifeManagement()
    {
        if (currentPlayer.life < 1) YouLoose();
    }

    public void TrainingManagement()
    {
        currentPlayer.legsTraining = treadmillTraining + bikeTraining + jumpboxTraining;
    }

    public void YouWin()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void YouLoose()
    {
        SceneManager.LoadScene("GameOver");
    }
}
