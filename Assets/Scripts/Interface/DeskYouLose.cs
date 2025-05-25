using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class DeskYouLose : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera playerCam;

    [SerializeField]
    Transform cameraPlace;

    [SerializeField]
    Transform cameraTarget;

    protected GameObject trainingPerson;

    protected string animationBool = "isGaming";

    protected Coroutine gamingCoroutine;

    [SerializeField]
    Transform gamingPosition;

    [SerializeField]
    GameObject panel;

    [SerializeField]
    IHM_youLose ihm_YouLose;

    // [SerializeField]
    // Transform screen;

    [SerializeField]
    AudioSource ambientAudio;

    [SerializeField]
    AudioSource keyboardAudio;


    public virtual void Interact(GameObject user)
    {
        trainingPerson = user;

        TakePlace();

        keyboardAudio.Play();

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

        StartCoroutine(StartGaming());
    }

    IEnumerator StartGaming()
    {
        yield return new WaitForSeconds(2f);

        // var transposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        // Vector3 currentOffset = transposer.m_FollowOffset;
        // transposer.m_FollowOffset = new Vector3(0f, 1.04f, 0.40f);
        // playerCam.LookAt = screen;
        // var composer = playerCam.GetCinemachineComponent<CinemachineComposer>();
        // Vector3 toCurrentOffset = composer.m_TrackedObjectOffset;
        // composer.m_TrackedObjectOffset = new Vector3(0f, -700f, 4f);

        // cameraTarget.localPosition = new Vector3(0f, 1.05f, 0.7f);
        // playerCam.Follow = cameraPlace;
        // cameraPlace.localPosition = new Vector3(0f, 1f, 0.5f);

        // CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        playerTransposer.m_FollowOffset = new Vector3(0f, 1.04f, 0.55f);

        panel.SetActive(true);

        keyboardAudio.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);

        cameraTarget.localPosition = new Vector3(0f, 1.04f, 0.7f);
    }

    void TakePlace()
    {
        // trainingPerson.transform.position = new Vector3(0f, 0f, 4.3f);
        // trainingPerson.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        trainingPerson.transform.position = gamingPosition.position;
        trainingPerson.transform.rotation = gamingPosition.rotation;
    }
}