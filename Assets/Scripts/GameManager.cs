using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player currentPlayer;


    void Awake()
    {
        instance = this;
        LaunchGame();
    }


    void LaunchGame()
    {
        currentPlayer = new Player("Kirill");
    }

}
