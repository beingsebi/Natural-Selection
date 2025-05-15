using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreatureSpawner : MonoBehaviour
{
    public GameObject agentPrefab;
    public int floorScale = 1;
    public int initialSpeed;
    public int initialViewDistance;
    public float mutationTemp;
    public float mutationChange;
    public float initialSize;
    private GameObject[] houseList;

    void Start()
    {
        houseList = GameObject.FindGameObjectsWithTag("House");
    }

    public void SpawnCreature()
    {
        if (houseList == null || houseList.Length == 0)
        {
            houseList = GameObject.FindGameObjectsWithTag("House");
        }

        GameObject randomHouse = houseList[Random.Range(0, houseList.Length)];

        Vector3 spawnOffset = new Vector3(
            Random.Range(-3f, 3f),
            0,
            Random.Range(-3f, 3f)
        );

        Vector3 spawnPosition = randomHouse.transform.position + spawnOffset;

        GameObject creature = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

        Creature creatureComponent = creature.GetComponent<Creature>();
        creatureComponent.speed = initialSpeed;
        creatureComponent.viewDistance = initialViewDistance;
        creatureComponent.mutationTemp = mutationTemp;
        creatureComponent.mutationChange = mutationChange;
        creatureComponent.size = initialSize;
    }
    public void SpawnPopulation(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnCreature();
        }
    }


    public void RemoveSpawned()
    {
        GameObject[] agentList = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject agent in agentList)
        {
            Destroy(agent);
        }
        Debug.Log("All agents destroyed.");
    }
}