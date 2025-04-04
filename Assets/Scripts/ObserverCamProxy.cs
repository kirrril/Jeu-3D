using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverCamProxy : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera observerCam;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = observerCam.State.RawPosition;
        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.deltaTime * 10f));
    }
}
