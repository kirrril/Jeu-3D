using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject proteinTub;

    [SerializeField]
    private GameObject waterBottle;


    void Start()
    {
        SpawnWater();

        SpawnProtein();
    }


    public void SpawnProtein()
    {
        StartCoroutine(SpawnProteinTub());
    }

    IEnumerator SpawnProteinTub()
    {
        yield return new WaitForSeconds(5);

        int spawnPointX = Random.Range(-10, 10);
        int spawnPointY = 2;
        int spawnPointZ = Random.Range(-10, 10);

        Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
        Instantiate(proteinTub, spawnPosition, Quaternion.identity);

    }

    public void SpawnWater()
    {
        StartCoroutine(SpawnWaterBottle());
    }

    IEnumerator SpawnWaterBottle()
    {
        yield return new WaitForSeconds(3);

        int spawnPointX = Random.Range(-5, 5);
        int spawnPointY = 2;
        int spawnPointZ = Random.Range(-5, 5);

        Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
        Instantiate(waterBottle, spawnPosition, Quaternion.identity);
    }
}
