using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinTub_old : MonoBehaviour// ICollectable
{
    public GameManager gameManager;

    [SerializeField]
    private int proteinContent = 5;
    
    public bool isCollectable {get {return true;}}

    public void Collect()
    {
        gameManager.currentPlayer.protein += proteinContent;

        IHM.instance.DisplayData();

        Destroy(gameObject);
    }

}
