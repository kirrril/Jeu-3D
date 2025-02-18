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
        Debug.Log($"Aucun item {itemName} n'est trouvé");

        return null;
    }

    IEnumerator SpawnItemCorout(GameObject item)
    {
        yield return new WaitForSeconds(5);

        int spawnPointX = Random.Range(-10, 10);
        int spawnPointY = 2;
        int spawnPointZ = Random.Range(-10, 10);

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
