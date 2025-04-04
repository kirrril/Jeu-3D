using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamProxy : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera playerCam;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = playerCam.State.RawPosition;
        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.deltaTime * 10f));
    }
}
