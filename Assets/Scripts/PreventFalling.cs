using UnityEngine;

public class PreventFalling : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
    }
}
