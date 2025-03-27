using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantData : MonoBehaviour
{
    public static PersistantData instance;

    public Data data;

    int alphaCoeff; 

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        data = new Data();
    }


    void Update()
    {
        GetData();
    }

    public void GetData()
    {
        data.elapsedTime = Timer.instance.time;
        data.proteinAmount = GameManager.instance.currentPlayer.protein;
        data.defeatedEnemies = GameManager.instance.currentPlayer.defeatedEnemies;

        data.alphaCoeff = CalculateAlphaCoeff();
    }

    public int CalculateAlphaCoeff()
    {
        int time = Convert.ToInt32(data.elapsedTime);

        if (time > 0)
        {
            alphaCoeff = (data.defeatedEnemies + data.proteinAmount) * 100 / time;
        }
        return alphaCoeff;
    }
}

[System.Serializable]
public class Data
{
    public float elapsedTime;

    public int proteinAmount;
    public int defeatedEnemies;
    public int alphaCoeff;
}
