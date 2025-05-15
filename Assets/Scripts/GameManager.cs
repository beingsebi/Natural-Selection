using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EpochStats
{
    public int epochNumber;
    public int startedCount;
    public int diedCount;
    public int survivedCount;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel; // Assign the parent UI object here in the Inspector

    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _sizeText;
    [SerializeField] private TextMeshProUGUI _senseText;
    [SerializeField] private TextMeshProUGUI _epochsText;
    [SerializeField] private TextMeshProUGUI _mutationTempText;
    [SerializeField] private TextMeshProUGUI _foodPerEpochText;
    [SerializeField] private TextMeshProUGUI _mutationChangeText;
    [SerializeField] private TextMeshProUGUI _energyPerEpochText;
    [SerializeField] private TextMeshProUGUI _populationPerEpochText;
    [SerializeField] private GameObject statLinePrefab;
    [SerializeField] private Transform statContentParent;
    [SerializeField] private GameObject statPanel;


    [SerializeField] private FoodSpawner _foodSpawner;
    [SerializeField] private CreatureSpawner _creatureSpawner; // reference to the creature spawner
    [SerializeField] private float creatureCheckInterval = 100.0f; // interval to check for creatures
    private bool _isRunning = false; // flag to check if the simulation is running
    private List<EpochStats> allEpochStats = new List<EpochStats>();

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
        float size = float.Parse(_sizeText.text);
        int sense = int.Parse(_senseText.text);

        int epochs = int.Parse(_epochsText.text);
        float mutationTemp = float.Parse(_mutationTempText.text);
        float mutationChange = float.Parse(_mutationChangeText.text);

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
        _creatureSpawner.initialSize = size;
        _creatureSpawner.mutationChange = mutationChange;
        // TODO add remaing characteristics

        Debug.Log($"Starting simulation with: Speed: {speed}, Size: {size}, Sense: {sense}, Epochs: {epochs}, Mutation Temp: {mutationTemp}, Population per Epoch: {populationPerEpoch}, Food per Epoch: {foodPerEpoch}, Energy per Epoch: {energyPerEpoch}");
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
        _creatureSpawner.SpawnPopulation(populationPerEpoch);
        yield return null;


        for (int i = 0; i < epochs; i++)
        {
            _foodSpawner.StartSpawning(foodPerEpoch);

            GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
            EpochStats stats = new EpochStats { epochNumber = i + 1 };


            foreach (GameObject agent in agents)
            {
                if (agent.TryGetComponent<Creature>(out var creature) && !creature.shouldBeDestroyed)
                {
                    stats.startedCount++;
                    // Count also the children that will be born during this epoch
                    if (creature.foodCount > 0)
                    {
                        stats.startedCount += creature.foodCount - 1;
                    }
                    creature.StartEpoch();
                }
            }

            yield return new WaitForSeconds(energyPerEpoch);

            List<GameObject> creaturesToDestroy = new List<GameObject>();
            foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent"))
            {
                if (agent.TryGetComponent<Creature>(out var creature))
                {
                    creature.EndEpoch();
                    if (creature.shouldBeDestroyed)
                    {
                        creaturesToDestroy.Add(agent);
                        stats.diedCount++;
                    }
                }
            }

            // Destroy the creatures that have eaten no food
            foreach (GameObject creature in creaturesToDestroy)
            {
                Destroy(creature);
            }

            int creaturesRemaining = stats.startedCount - stats.diedCount;
            Debug.Log($"Epoch {i + 1}: Creatures remaining: {creaturesRemaining}");
            _foodSpawner.RemoveSpawned();

            stats.survivedCount = creaturesRemaining;
            allEpochStats.Add(stats);

            // If there are no creatures remaining, break the loop
            if (creaturesRemaining == 0)
            {
                Debug.Log("Epoch finished: No creatures remaining.");
                break;
            }
        }

        ShowEpochStats();
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
    private void ShowEpochStats()
    {
        statPanel.SetActive(true); // Afișează scroll-ul

        foreach (EpochStats stats in allEpochStats)
        {
            GameObject line = Instantiate(statLinePrefab, statContentParent);
            TMP_Text text = line.GetComponent<TMP_Text>();
            text.text = $"Epoch {stats.epochNumber}: Start = {stats.startedCount}, Died = {stats.diedCount}, Survived = {stats.survivedCount}";
        }
    }
    public void ReturnToMenu()
    {
        // Ascunde panoul cu statistici
        statPanel.SetActive(false);

        // Reafișează meniul cu slider-ele
        _settingsPanel.SetActive(true);

        // Șterge statisticile salvate (pregătire pentru o nouă simulare)
        allEpochStats.Clear();

        // (Opțional) Șterge vizual toate liniile din scroll
        foreach (Transform child in statContentParent)
        {
            Destroy(child.gameObject);
        }
    }


}