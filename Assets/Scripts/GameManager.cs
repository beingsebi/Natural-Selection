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


        
    }
}
