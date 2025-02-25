using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TrainingMachineBase : MonoBehaviour, IInteractable
{
    private Transform trainingPosition;

    protected Transform stopTrainingPosition;

    protected GameObject trainingPerson;

    protected float thisMachineTraining = 0;

    public virtual bool isInteractable { get; set; } = true;


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
        if (other.CompareTag("Player") || other.CompareTag("Girl") || other.CompareTag("Man"))
        {
            trainingPerson = other.gameObject;

            Train(trainingPerson.transform);

            isInteractable = false;

            Debug.Log($"Training person : {other.gameObject}");
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
}
