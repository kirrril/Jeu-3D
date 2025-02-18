using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter()
    {
        if (GameManager.instance.currentPlayer.life > 0)
        {
            GameManager.instance.currentPlayer.life -= 1;
            PlayerController.instance.StartPosition();

        } else
        {
            GameManager.instance.YouLoose();
        }

    }


}
