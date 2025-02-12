using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        
    }

    void LaunchGame()
    {
        currentPlayer = new Player("Kirill");
    }

}
