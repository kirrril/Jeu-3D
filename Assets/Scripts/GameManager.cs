using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player currentPlayer;


    void Awake()
    {
        instance = this;
        LaunchGame();
    }

    void Update()
    {
        LifeManagement();
    }


    void LaunchGame()
    {
        currentPlayer = new Player("Kirill");
    }

    void LifeManagement()
    {
        if (currentPlayer.life >= 1)
        {
            if (currentPlayer.water <= 0)
            {
                currentPlayer.life -= 1;
            }
        }
        
        if (currentPlayer.life < 1) YouLoose();
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
