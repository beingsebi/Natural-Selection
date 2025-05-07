using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject agentPrefab;
    public int floorScale = 1;
    public int initialSpeed ;
    public int initialViewDistance ;
    public float mutationTemp;

    public void SpawnCreature()
    {
        int x = Random.Range(-35, 35) * floorScale;
        int z = Random.Range(-35, 35) * floorScale;
        GameObject creature = Instantiate(agentPrefab, new Vector3((float)x, 1, (float)z), Quaternion.identity);
        creature.GetComponent<Creature>().speed = initialSpeed;
        creature.GetComponent<Creature>().viewDistance = initialViewDistance;
        creature.GetComponent<Creature>().mutationRate = mutationTemp;
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