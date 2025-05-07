using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public int floorScale = 1;
    public GameObject myPrefab;

    public void StartSpawning(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnFood();
        }
    }

    void SpawnFood()
    {
        int x = Random.Range(-35, 35) * floorScale;
        int z = Random.Range(-35, 35) * floorScale;
        Instantiate(myPrefab, new Vector3((float)x, 1, (float)z), Quaternion.identity);
    }

    public void RemoveSpawned()
    {
        GameObject[] foodList = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foodList)
        {
            Destroy(food);
        }
        Debug.Log("All food destroyed.");
    }
}