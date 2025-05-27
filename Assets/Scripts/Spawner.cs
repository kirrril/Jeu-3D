using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    List<ItemInfo> itemsList;

    [System.Serializable]
    public class ItemInfo
    {
        public string itemName;

        public GameObject itemPrefab;
    }

    [SerializeField]
    List<BoxCollider> spawnZones1;

    [SerializeField]
    List<BoxCollider> spawnZones2;

    [SerializeField]
    List<BoxCollider> spawnZones3;


    void Start()
    {
        SpawnItem("Protein");

        SpawnItem("Water");
    }


    public void SpawnItem(string itemName)
    {
        GameObject itemPrefab = GetItemByName(itemName);

        StartCoroutine(SpawnItemCorout(itemPrefab));
    }

    GameObject GetItemByName(string itemName)
    {
        foreach (ItemInfo itemInfo in itemsList)
        {
            if (itemInfo.itemName == itemName)
            {
                return itemInfo.itemPrefab;
            }
        }

        return null;
    }

    IEnumerator SpawnItemCorout(GameObject item)
    {
        yield return new WaitForSeconds(3);

        BoxCollider spawnZone = null;

        if (GameManager.instance.currentPlayer.level == 1)
        {
            spawnZone = spawnZones1[Random.Range(0, spawnZones1.Count)];
        }
        else if (GameManager.instance.currentPlayer.level == 2)
        {
            spawnZone = spawnZones2[Random.Range(0, spawnZones2.Count)];
        }
        else if (GameManager.instance.currentPlayer.level == 3 || GameManager.instance.currentPlayer.level == 0)
        {
            spawnZone = spawnZones3[Random.Range(0, spawnZones3.Count)];
        }

        Vector3 spawnPosition = GetRandomPointInCollider(spawnZone);

        Instantiate(item, spawnPosition, Quaternion.identity);

    }

    private Vector3 GetRandomPointInCollider(BoxCollider collider)
    {
        Bounds bounds = collider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = bounds.min.y + 0.2f;
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}

