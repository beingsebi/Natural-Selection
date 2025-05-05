using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject agentPrefab;
    private GameObject[] agentList;
    public int floorScale = 1;

    public void StartSpawning(int spawnCount)
    {
        // Spawn a specified number of agents at random locations at the start of the game
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnCreature();
        }
    }

    void SpawnCreature()
    {
        int x = Random.Range(-35, 35) * floorScale;
        int z = Random.Range(-35, 35) * floorScale;
        Instantiate(agentPrefab, new Vector3((float)x, 0.75f, (float)z), Quaternion.identity);
    }

    public void RemoveSpawned()
    {
        agentList = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject agent in agentList)
        {
            Destroy(agent);
        }
        Debug.Log("All agents destroyed.");
    }
}