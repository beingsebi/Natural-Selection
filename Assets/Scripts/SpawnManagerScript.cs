using UnityEngine;
using System.Collections.Generic;
public class SpawnManagerScript : MonoBehaviour
{
    
    public Transform[] spawnPoints;

    public void assignRandomPositions(GameObject[] players)
    {
        if (spawnPoints.Length >= players.Length)
        {
            List<int> usedIndices = new List<int>();
            for (int i = 0; i < players.Length; i++)    
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, spawnPoints.Length);
                } while (usedIndices.Contains(randomIndex));
                
                usedIndices.Add(randomIndex);
                players[i].GetComponent<Traits>().position = spawnPoints[randomIndex];
            }
        }
        else
        {
            Debug.LogError("Not enough spawn points for all players");
        }
    }
}
