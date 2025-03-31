using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinTub : MonoBehaviour, ICollectable
{
    private Spawner spawner;

    [SerializeField]
    private GameObject proteinTub;

    [SerializeField]
    AudioSource proteinCollect;

    public bool isCollectable { get { return true; } }


    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }

        if (other.CompareTag("Ground"))
        {
            StartCoroutine(Expire());
        }
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);

        spawner.SpawnItem("Protein");
    }

    public void Collect()
    {
        proteinCollect.Play();

        GameManager.instance.currentPlayer.protein += 1;

        Destroy(gameObject);

        spawner.SpawnItem("Protein");
    }
}

