using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public GameObject agentPrefab;
    public CharacterController controller;
    public bool isActive = false;
    public int foodCount = 0;
    public float mutationTemp = 0.5f; // Chance of mutation
    public float mutationChange = 0.1f; // How much genes can change on a single mutation

    // Genes that can be mutated
    public float viewDistance = 5;
    public float size = 1.0f;
    public float speed = 10f;
    // End of genes

    public float eatThreshold = 1.5f;

    public bool hasFoodInRange = false;


    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        this.name = "Agent";
        foodCount = 0;

        speed = MutateGene(speed);
        // Clamp size between 1 and 2
        size = Mathf.Clamp(MutateGene(size), 1f, 2f);
        viewDistance = MutateGene(viewDistance);

        // Adjust the size of the creature
        transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive)
        {
            MoveToFood();
        }
    }

    void MoveToFood()
    {
        GameObject closestFood = FindClosestFood();
        if (closestFood != null)
        {
            hasFoodInRange = true;
            Vector3 directionToFood = closestFood.transform.position - transform.position;
            directionToFood.y = 0; // Ignore vertical difference

            directionToFood.Normalize();
            transform.forward = directionToFood;

            // Create movement vector with no vertical component
            Vector3 moveDirection = transform.forward * speed * Time.deltaTime;
            moveDirection.y = 0;

            // Apply movement
            controller.Move(moveDirection);

            // Force Y position to ground level
            Vector3 pos = transform.position;
            pos.y = 1 + size / 3;
            transform.position = pos;
        }
        else
        {
            hasFoodInRange = false;
            // Check boundaries and reverse direction if needed
            if (transform.position.x < -35f || transform.position.x > 35f)
            {
                transform.forward = new Vector3(-transform.forward.x, 0, transform.forward.z);
            }
            if (transform.position.z < -35f || transform.position.z > 35f)
            {
                transform.forward = new Vector3(transform.forward.x, 0, -transform.forward.z);
            }

            // If no food is found, move randomly but stay within bounds
            transform.Rotate(Vector3.up, UnityEngine.Random.Range(0, 90) * Time.deltaTime);

            // Create movement vector with no vertical component
            Vector3 moveDirection = transform.forward * speed * Time.deltaTime;
            moveDirection.y = 0;

            // Apply movement
            controller.Move(moveDirection);

            // Clamp position to boundaries and ground level
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, -35f, 35f);
            pos.y = 1 + size / 3;
            pos.z = Mathf.Clamp(pos.z, -35f, 35f);
            transform.position = pos;
        }
    }

    //this function gets called whenever the agent collides with a trigger. (Which in this case is the food)
    void OnTriggerEnter(Collider col)
    {
        //if the agent collides with a food object, it will eat it
        if (col.gameObject.tag == "Food" && isActive)
        {
            foodCount = foodCount + 1;
            Destroy(col.gameObject);

        }


        if (col.gameObject.CompareTag("Agent"))
        {
            float otherSize = Mathf.Round(col.gameObject.GetComponent<Creature>().size * 1000f) / 1000f;
            float thresholdSize = Mathf.Round(size * 1000f) / 1000f;
            if (otherSize * eatThreshold < thresholdSize)
            {
                foodCount = foodCount + 1;
                Destroy(col.gameObject);
                Debug.Log("Eaten a smaller creature");
            }
        }
    }
    GameObject FindClosestFood()
    {
        // Find all food objects within viewDistance radius using OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, viewDistance);
        GameObject closestFood = null;
        float minDistance = float.MaxValue;

        foreach (Collider col in hitColliders)
        {
            // Check if the collider belongs to a food object
            if (col.gameObject.CompareTag("Food"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestFood = col.gameObject;
                }
            }
            if (col.gameObject.CompareTag("Agent"))
            {
                if (col.gameObject.GetComponent<Creature>().size * eatThreshold < size)
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestFood = col.gameObject;
                    }
                }
            }
        }
        return closestFood;
    }
    public void StartEpoch()
    {
        Reproduce();
        foodCount = 0;
        isActive = true;
    }

    public void EndEpoch()
    {
        isActive = false;
        // Debug.Log("Food count: " + foodCount);
        if (foodCount <= 0)
        {
            Destroy(gameObject); // Destroy the creature if it has eaten food
        }
        foodCount -= 1;
    }

    // It reproduces if it has eaten more than 1 food
    void Reproduce()
    {
        for (int i = 0; i < foodCount; i++)
        {
            // Add random offset to spawn position
            Vector3 spawnOffset = new Vector3(
                UnityEngine.Random.Range(-5f, 5f),
                0,
                0
            );
            Vector3 spawnPosition = transform.position + spawnOffset;

            GameObject newCreature = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);
            Creature childCreature = newCreature.GetComponent<Creature>();

            childCreature.isActive = true;
        }
    }

    float MutateGene(float gene)
    {
        if (UnityEngine.Random.value < mutationTemp)
        {
            float mutation = UnityEngine.Random.Range(-mutationChange, mutationChange);
            return Mathf.Max(0.1f, gene * (1 + mutation)); // Ensure values don't go below 0.1
        }
        return gene;
    }
}