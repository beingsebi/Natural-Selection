using UnityEngine;

public class PreventFalling : MonoBehaviour
{
    private float maxRotation = 5f;

    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
    }
    void Update()
    {
        
    }
}
