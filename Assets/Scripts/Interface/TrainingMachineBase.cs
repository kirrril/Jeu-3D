using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TrainingMachineBase : MonoBehaviour, IInteractable
{
    protected Transform trainingPosition;

    protected Transform stopTrainingPosition;

    protected GameObject trainingPerson;

    public string animationName;

    [SerializeField]
    protected float trainingDuration = 5.0f;

    protected float thisMachineTraining = 0;

    public virtual bool isInteractable { get {return trainingPerson == null;} set {} }

    public bool isInteracting { get {return trainingPerson != null;} }

    protected void Start()
    {
        trainingPosition = transform.Find("TrainingPos");
        stopTrainingPosition = transform.Find("StopTrainingPos");
    }


    protected virtual void Update()
    {
        WaterManagement();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact(other.gameObject, null);
        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        isInteractable = true;
    }


    protected void Train(Transform trainingPerson)
    {
        if (!isInteractable) return;

        trainingPerson.position = trainingPosition.position;

        trainingPerson.rotation = trainingPosition.rotation;

        if (trainingPerson.CompareTag("Player"))
        {
            IHM.instance.stopTrainingButton.gameObject.SetActive(true);

            IHM.instance.stopTrainingButton.onClick.AddListener(OnButtonClick);
        }
    }


    protected void StopTraining()
    {
        if (trainingPerson.CompareTag("Player"))
        {
            IHM.instance.stopTrainingButton.gameObject.SetActive(false);

            trainingPerson.transform.position = stopTrainingPosition.position;

            trainingPerson.transform.rotation = stopTrainingPosition.rotation;
        }
    }


    protected void OnButtonClick()
    {
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

    public virtual void Interact(GameObject user, System.Action callBack)
    {
        trainingPerson = user;

        Train(trainingPerson.transform);

        isInteractable = false;

        Debug.Log($"Training person : {user}");

        StartCoroutine(TrainingCorout(user, callBack));
    }

    protected virtual IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        yield return null;
    }
}
