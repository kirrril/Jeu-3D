using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TrainingMachineBase : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected Transform trainingPosition;
    [SerializeField]
    protected Transform stopTrainingPosition;

    protected GameObject trainingPerson;

    public string animationName;

    [SerializeField]
    protected float trainingDuration = 1.0f;

    protected float thisMachineTraining = 0;

    public virtual bool isInteractable { get { return trainingPerson == null; } set { } }

    public bool isInteracting { get { return trainingPerson != null; } }


    protected virtual void Update()
    {
        WaterManagement();
    }


    public virtual void Interact(GameObject user, System.Action callBack)
    {
        if (!isInteractable) return;

        trainingPerson = user;

        TakePlace();

        if (trainingPerson.name == "Player")
        {
            StopTrainingButtonOn();
        }

        if (trainingPerson.name != "Player")
        {
            StartCoroutine(TrainingCorout(user, LeavePlace));

            Debug.Log("Coroutine started");
        }

        isInteractable = false;
    }


    protected virtual IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        yield return null;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject, null);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerController.instance.isTraining = false;

        isInteractable = true;
    }


    void StopTrainingButtonOn()
    {
        IHM.instance.stopTrainingButton.gameObject.SetActive(true);

        IHM.instance.stopTrainingButton.onClick.AddListener(OnButtonClick);
    }

    protected virtual void StopTraining()
    {
        IHM.instance.stopTrainingButton.gameObject.SetActive(false);

        // PlayerController.instance.isTraining = false;
    }

    void TakePlace()
    {
        trainingPerson.transform.position = trainingPosition.position;
        trainingPerson.transform.rotation = trainingPosition.rotation;
    }

    protected void LeavePlace()
    {
        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;
    }

    protected void OnButtonClick()
    {
        LeavePlace();
        StopTraining();
    }




    protected void WaterManagement()
    {
        if (PlayerController.instance.isTraining == true)
        {
            float waterLoss = Time.deltaTime / 100;

            GameManager.instance.currentPlayer.water -= waterLoss;
        }

        if (GameManager.instance.currentPlayer.water <= 0)
        {
            PlayerController.instance.StartPosition();

            IHM.instance.stopTrainingButton.gameObject.SetActive(false);

            GameManager.instance.currentPlayer.life--;

            GameManager.instance.currentPlayer.water = 0.5f;
        }
    }


}
