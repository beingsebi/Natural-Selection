using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel; // Assign the parent UI object here in the Inspector

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
    [SerializeField] private float creatureCheckInterval = 100.0f; // interval to check for creatures
    private bool _isRunning = false; // flag to check if the simulation is running

    // [SerializeField] private SpawnManagerScript _spawnManager; // will use _spawnManager.spawnPoints
    // [SerializeField] private GameObject _playerPrefab;

    public void EndGame()
    {
        if (!_isRunning)
        {
            Debug.Log("Game is not running, cannot quit.");
            return; // Exit if the game is not running
        }

        Debug.Log("Game is quitting...");
        CleanUp(); // Clean up before quitting
        Application.Quit();
    }

    public void StartSimulation()
    {
        if (_isRunning)
        {
            Debug.Log("Simulation is already running.");
            return; // Exit if the simulation is already running
        }
        _isRunning = true;
        _settingsPanel.SetActive(false); // Hide the settings UI

        int speed = int.Parse(_speedText.text);
        int strength = int.Parse(_strengthText.text);
        int sense = int.Parse(_senseText.text);

        int epochs = int.Parse(_epochsText.text);
        float mutationTemp = float.Parse(_mutationTempText.text);

        int populationPerEpoch = int.Parse(_populationPerEpochText.text);
        int foodPerEpoch = int.Parse(_foodPerEpochText.text);
        int energyPerEpoch = int.Parse(_energyPerEpochText.text);

        if (_foodSpawner == null || _creatureSpawner == null)
        {
            Debug.LogError("Spawner reference(s) not set in GameManager!");
            return;
        }
        _creatureSpawner.initialSpeed = speed;
        _creatureSpawner.initialViewDistance = sense;
        _creatureSpawner.mutationTemp = mutationTemp;
        // TODO add remaing characteristics

        Debug.Log($"Starting simulation with: Speed: {speed}, Strength: {strength}, Sense: {sense}, Epochs: {epochs}, Mutation Temp: {mutationTemp}, Population per Epoch: {populationPerEpoch}, Food per Epoch: {foodPerEpoch}, Energy per Epoch: {energyPerEpoch}");
        StartCoroutine(SimulateEpochs(epochs, populationPerEpoch, foodPerEpoch, energyPerEpoch));

        // save stats to disk
        // display stats 
        // add some menu for stats 
    }

    // The creatures are spawned and when the epoch starts, the creatures are activated and move to the food
    // The creatures reproduce if they have eaten more than 1 food
    // The creatures die if they have eaten no food
    // The creatures reproduce if they have eaten more than 1 food
    private IEnumerator SimulateEpochs(int epochs, int populationPerEpoch, int foodPerEpoch, int energyPerEpoch)
    {
        for (int i = 0; i < populationPerEpoch; i++)
        {
            _creatureSpawner.SpawnCreature();
        }

        for (int i = 0; i < epochs; i++)
        {
            _foodSpawner.StartSpawning(foodPerEpoch);
            foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent"))
            {
                if (agent.TryGetComponent<Creature>(out var creature))
                {
                    creature.StartEpoch();
                }
                else
                {
                    Debug.LogError("Creature component not found on agent!");
                }
            }

        yield return new WaitForSeconds(energyPerEpoch);


            foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent"))
            {
                if (agent.TryGetComponent<Creature>(out var creature))
                {
                    creature.EndEpoch();
                }
                else
                {
                    Debug.LogError("Creature component not found on agent!");
                }
            }
            int creaturesRemaining = GameObject.FindGameObjectsWithTag("Agent").Length;
            Debug.Log($"Epoch {i + 1}: Creatures remaining: {creaturesRemaining}");
            _foodSpawner.RemoveSpawned();

            if (creaturesRemaining == 0)
            {
                Debug.Log("Epoch finished: No creatures remaining.");
                break;
            }
        }

        CleanUp();
    }

    private void CleanUp()
    {
        _foodSpawner.RemoveSpawned();
        _creatureSpawner.RemoveSpawned();
        _isRunning = false;

        if (_settingsPanel != null)
        {
            _settingsPanel.SetActive(true); // Show the settings UI again
        }
    }
}