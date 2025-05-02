using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlates : MonoBehaviour
{
    [SerializeField]
    List<GameObject> WeightPlates;

    [SerializeField]
    BoxCollider spawnZone;

    int plateIndex;

    GameObject spawnedPlate;

    Coroutine spawnCorout;


    public void SpawnWeightPlates()
    {
        spawnCorout = StartCoroutine(SpawnItemCorout());
    }

    public void StopSpawningWeightPlates()
    {
        if (spawnCorout != null)
        {
            StopCoroutine(spawnCorout);
            spawnCorout = null;
        }

        if (spawnedPlate != null)
        {
            Destroy(spawnedPlate);
        }
    }

    IEnumerator SpawnItemCorout()
    {
        while (GameManager.instance.currentPlayer.level == 3)
        {
            yield return new WaitForSeconds(5f);

            if (GameManager.instance.currentPlayer.level != 3)
            {
                spawnCorout = null;
                yield break;
            }

            Vector3 spawnPosition = GetRandomPointInCollider(spawnZone);

            spawnedPlate = Instantiate(WeightPlates[ChoseRandomPlateIndex()], spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(10);

            if (spawnedPlate != null)
            {
                Destroy(spawnedPlate);
                spawnedPlate = null;
            }
        }

        spawnCorout = null;
    }

    int ChoseRandomPlateIndex()
    {
        plateIndex = Random.Range(0, WeightPlates.Count);

        return plateIndex;
    }

    private Vector3 GetRandomPointInCollider(BoxCollider spawnZone)
    {
        Bounds bounds = spawnZone.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float heightY = bounds.min.y + 20f;
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, heightY, randomZ);
    }
}
