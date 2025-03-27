using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class Desk : MonoBehaviour
{
    [SerializeField]
    protected Transform gamingPosition;

    protected GameObject trainingPerson;

    protected string animationBool = "isGaming";

    protected Coroutine gamingCoroutine;

    [SerializeField]
    CinemachineVirtualCamera playerCam;

    [SerializeField]
    Transform screen;


    public virtual void Interact(GameObject user)
    {
        trainingPerson = user;

        TakePlace();

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

        StartCoroutine(TryAgainCorout());
    }

    IEnumerator TryAgainCorout()
    {
        yield return new WaitForSeconds(2f);

        var transposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 currentOffset = transposer.m_FollowOffset;
        transposer.m_FollowOffset = new Vector3(0f, 1.04f, 0.40f);
        playerCam.LookAt = screen;
        var composer = playerCam.GetCinemachineComponent<CinemachineComposer>();
        Vector3 toCurrentOffset = composer.m_TrackedObjectOffset;
        composer.m_TrackedObjectOffset = new Vector3(0f, -700f, 4f);

        yield return new WaitForSeconds(0.5f);

        IHM_youLose.instance.panel.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }

    void TakePlace()
    {
        // trainingPerson.transform.position = gamingPosition.position;
        // trainingPerson.transform.rotation = gamingPosition.rotation;

        // trainingPerson.transform.position = new Vector3(0f, 0f, 2.7f);
        // trainingPerson.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}