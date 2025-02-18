using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private bool isGrounded;
    private float chargeJump;


    void Update()
    {
        if (isGrounded && PlayerController.isReadyToJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                chargeJump += Time.deltaTime * 100;

                chargeJump = Mathf.Clamp(chargeJump, 0, 20);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity = (transform.forward * chargeJump * GameManager.instance.currentPlayer.legsTraining) + (transform.up * chargeJump * GameManager.instance.currentPlayer.legsTraining);
                isGrounded = false;
                chargeJump = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayerController.isReadyToJump = false;

            isGrounded = true;
        }
    }
}
