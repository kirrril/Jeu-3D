using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player currentPlayer;

    void Awake()
    {
        LaunchGame();
    }

    void Start()
    {
        
    }

    void LaunchGame()
    {
        currentPlayer = new Player(IHM_StartGame.playersName);
    }

}
