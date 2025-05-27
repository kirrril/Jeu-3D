using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeskStart : MonoBehaviour
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
    IHM_startGame ihm_StartGame;

    [SerializeField]
    Transform screen;

    [SerializeField]
    AudioSource ambientAudio;

    [SerializeField]
    AudioSource keyboardAudio;


    public virtual void Interact(GameObject user)
    {
        trainingPerson = user;

        TakePlace();

        CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        playerTransposer.m_FollowOffset = new Vector3(0f, 2.1f, -0.9f);

        keyboardAudio.Play();

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

        StartCoroutine(StartGaming());
    }

    IEnumerator StartGaming()
    {
        yield return new WaitForSeconds(2f);

        CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        playerTransposer.m_FollowOffset = new Vector3(0f, 1.04f, 0.55f);

        panel.SetActive(true);
        ihm_StartGame.FocusInputField();

        keyboardAudio.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);

        cameraTarget.localPosition = new Vector3(0f, 1.04f, 0.7f);
    }

    void TakePlace()
    {
        trainingPerson.transform.position = gamingPosition.position;
        trainingPerson.transform.rotation = gamingPosition.rotation;
    }
}
