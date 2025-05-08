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


    public void SpawnCreature()
    {
        int x = Random.Range(-35, 35) * floorScale;
        int z = Random.Range(-35, 35) * floorScale;
        // Debug.Log("Spawning creature...");
        GameObject creature = Instantiate(agentPrefab, new Vector3(x, 1, z), Quaternion.identity);
        // Debug.Log("Creature spawned, setting genes...");
        creature.GetComponent<Creature>().speed = initialSpeed;
        creature.GetComponent<Creature>().viewDistance = initialViewDistance;
        creature.GetComponent<Creature>().mutationTemp = mutationTemp;
        creature.GetComponent<Creature>().mutationChange = mutationChange;
        creature.GetComponent<Creature>().size = initialSize;
        // Debug.Log("Genes set. Creature ready.");
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