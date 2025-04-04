using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playersName;

    public int life = 5;

    public int protein = 0;

    public float water = 0.5f;

    public float legsTraining = 0f;

    public float chestTraining = 1f;

    public float backTraining = 0f;

    public int level;

    public int defeatedEnemies;

    public Player(string playersName)
    {
        this.playersName = playersName;
    }
}
