using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _strengthText;
    [SerializeField] private TextMeshProUGUI _senseText;
    [SerializeField] private TextMeshProUGUI _epochsText;
    [SerializeField] private TextMeshProUGUI _mutationTempText;
    [SerializeField] private TextMeshProUGUI _foodPerEpochText;
    [SerializeField] private TextMeshProUGUI _energyPerEpochText;
    [SerializeField] private TextMeshProUGUI _populationPerEpochText;

    [SerializeField] private FoodSpawner _foodSpawner; 
    [SerializeField] private CreatureSpawner _creatureSpawner; // reference to the creature spawner
    [SerializeField] private float creatureCheckInterval = 0.6f; // interval to check for creatures

    // [SerializeField] private SpawnManagerScript _spawnManager; // will use _spawnManager.spawnPoints
    // [SerializeField] private GameObject _playerPrefab;
    
    public void EndGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }

    public void StartSimulation()
    {
        int speed = int.Parse(_speedText.text);
        int strength = int.Parse(_strengthText.text);
        int sense = int.Parse(_senseText.text);

        int epochs = int.Parse(_epochsText.text);
        int mutationTemp = int.Parse(_mutationTempText.text);

        int populationPerEpoch = int.Parse(_populationPerEpochText.text);
        int foodPerEpoch = int.Parse(_foodPerEpochText.text);
        int energyPerEpoch = int.Parse(_energyPerEpochText.text);

        Debug.Log($"Starting simulation with: Speed: {speed}, Strength: {strength}, Sense: {sense}, Epochs: {epochs}, Mutation Temp: {mutationTemp}, Population per Epoch: {populationPerEpoch}, Food per Epoch: {foodPerEpoch}, Energy per Epoch: {energyPerEpoch}");
        for (int i = 0; i < epochs; i++)
        {
            StartCoroutine(SimulateEpoch(populationPerEpoch, foodPerEpoch, mutationTemp));
            // gather stats here 
        }
        // save stats to disk
        // display stats 
        // add some menu for stats 
    }
    
    private IEnumerator SimulateEpoch(int populationPerEpoch, int foodPerEpoch, int mutationTemp)
    {
        // Ensure references are set
        if (_foodSpawner == null || _creatureSpawner == null)
        {
            Debug.LogError("Spawner reference(s) not set in GameManager!");
            yield break; // Exit the coroutine
        }

        // Start spawning food and creatures
        _foodSpawner.StartSpawning(foodPerEpoch);
        _creatureSpawner.StartSpawning(populationPerEpoch); 

        // --- Wait for creatures to be "dead" ---
        Debug.Log("Epoch running, waiting for creatures to die...");
        // Check if creatures exist initially before starting the loop
        bool creaturesExist = GameObject.FindGameObjectsWithTag("Agent").Length > 0;

        while (creaturesExist)
        {
            Debug.Log("Creatures are still alive, waiting...");
            // Wait for the specified interval before checking again
            yield return new WaitForSeconds(creatureCheckInterval); 

            // Check if any agents are left
            creaturesExist = GameObject.FindGameObjectsWithTag("Agent").Length > 0;
        }
        Debug.Log("All creatures are dead.");
        // --- End Wait ---
        
        // Stop spawners and clean up
        _foodSpawner.RemoveSpawned(); 
        _creatureSpawner.RemoveSpawned(); 
    }
}