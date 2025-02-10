using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playersName;

    public int life = 5;

    public int protein = 5;

    public float water = 0.5f;

    public int personalGrowth = 1;

    public Player(string playersName)
    {
        this.playersName = playersName;
    }
}
