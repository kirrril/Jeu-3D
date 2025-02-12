using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour, ICollectable
{
    public GameManager gameManager;

    [SerializeField]
    private float waterContent = 1.0f;

    public bool isCollectable { get { return true; } }

    public void Collect()
    {
        gameManager.currentPlayer.water = waterContent;

        IHM.instance.DisplayData();

        Destroy(gameObject);
    }
}
