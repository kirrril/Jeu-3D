using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    List<ItemInfo> itemsList;


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
        float spawnPointX = 0f;
        float spawnPointY = 0f;
        float spawnPointZ = 0f;

        yield return new WaitForSeconds(3);

        if (GameManager.instance.currentPlayer.level == 1)
        {
            spawnPointX = Random.Range(-9f, 9f);
            spawnPointY = 0.1f;
            spawnPointZ = Random.Range(-9f, 9f);

            if (spawnPointX < 3f && spawnPointZ > 3f)
            {
                spawnPointX = Random.Range(-9f, 9f);
                spawnPointZ = Random.Range(-9f, 9f);
            }
        }

        if (GameManager.instance.currentPlayer.level == 2)
        {
            spawnPointX = Random.Range(-21f, -7f);
            spawnPointY = 3.4f;
            spawnPointZ = Random.Range(23f, 50f);
        }

        if (GameManager.instance.currentPlayer.level == 3)
        {
            spawnPointX = Random.Range(-3.5f, 18f);
            spawnPointY = -8f;
            spawnPointZ = Random.Range(58f, 76f);
        }

        Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
        Instantiate(item, spawnPosition, Quaternion.identity);

    }
}

[System.Serializable]
public class ItemInfo
{
    public string itemName;

    public GameObject itemPrefab;
}
