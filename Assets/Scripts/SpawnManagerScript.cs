using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    
    public Transform[] spawnPoints;

    private void Update()
    {
        foreach (Transform point in spawnPoints)
        {
            point.position = new Vector3(point.position.x, 0, point.position.z);
        }
    }
}
