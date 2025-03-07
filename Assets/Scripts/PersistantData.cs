using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantData : MonoBehaviour
{
    public static PersistantData instance;

    public Data data;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        data = new Data();
    }

    public void GetData()
    {
        data.elapsedTime = Timer.instance.time;
    }

}

[System.Serializable]
public class Data
{
    public float sweatAmount;

    public float elapsedTime;

    public float proteinAmount;

    public float submissedEnemies;
}
