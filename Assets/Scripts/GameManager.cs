using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _strengthText;
    [SerializeField] private TextMeshProUGUI _senseText;

    [SerializeField] private TextMeshProUGUI _populationText;
    [SerializeField] private TextMeshProUGUI _epochsText;
    [SerializeField] private TextMeshProUGUI _mutationTempText;
    [SerializeField] private TextMeshProUGUI _foodPerEpochText;
    [SerializeField] private TextMeshProUGUI _energyPerEpochText;

    [SerializeField] private SpawnManagerScript _spawnManager; // will use _spawnManager.spawnPoints
    [SerializeField] private GameObject _playerPrefab;
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

        int population = int.Parse(_populationText.text);
        int epochs = int.Parse(_epochsText.text);
        int mutationTemp = int.Parse(_mutationTempText.text);

        int foodPerEpoch = int.Parse(_foodPerEpochText.text);
        int energyPerEpoch = int.Parse(_energyPerEpochText.text);

        GameObject[] players = new GameObject[population];
        for (int i = 0; i < population; i++)
        {
            players[i] = Instantiate(_playerPrefab);
            players[i].GetComponent<Traits>().speed = speed;
            players[i].GetComponent<Traits>().strength = strength;
            players[i].GetComponent<Traits>().sense = sense;
            players[i].GetComponent<Traits>().energy = energyPerEpoch;
        }

        _spawnManager.assignRandomPositions(players);

        for (int i = 0; i < epochs; i++)
        {
            players = simulateEpoch(players, foodPerEpoch, mutationTemp);
            // gather stats here 
        }
        // display stats here 
    }

    private GameObject[] simulateEpoch(GameObject[] players, int foodPerEpoch, int mutationTemp)
    {
        // simulate the epoch
        return players;
    }
}
