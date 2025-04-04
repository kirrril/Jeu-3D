using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeskStart : MonoBehaviour
{
    protected GameObject trainingPerson;

    protected string animationBool = "isGaming";

    protected Coroutine gamingCoroutine;

    [SerializeField]
    GameObject panel;

    [SerializeField]
    IHM_startGame ihm_StartGame;

    [SerializeField]
    CinemachineVirtualCamera playerCam;

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

        keyboardAudio.Play();

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

        StartCoroutine(StartGaming());
    }

    IEnumerator StartGaming()
    {
        yield return new WaitForSeconds(2f);

        var transposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 currentOffset = transposer.m_FollowOffset;
        transposer.m_FollowOffset = new Vector3(0f, 1.04f, 0.40f);
        playerCam.LookAt = screen;
        var composer = playerCam.GetCinemachineComponent<CinemachineComposer>();
        Vector3 toCurrentOffset = composer.m_TrackedObjectOffset;
        composer.m_TrackedObjectOffset = new Vector3(0f, -700f, 4f);

        panel.SetActive(true);
        ihm_StartGame.FocusInputField();

        keyboardAudio.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }

    void TakePlace()
    {
        trainingPerson.transform.position = new Vector3(0f, 0f, 4.3f);
        trainingPerson.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
