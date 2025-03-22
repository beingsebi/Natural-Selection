using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public GameObject backgroundPanel;

    void Start()
    {
        if (backgroundPanel != null)
        {

            if (backgroundPanel != null)
            {
                backgroundPanel.SetActive( false);
            }
            else
            {
                Debug.LogWarning("Background panel does not have an Image component.");
            }
        }
        else
        {
            Debug.LogWarning("Background panel is not assigned.");
        }
    }
    public void ToggleMenu()
    {
        if (backgroundPanel != null)
        {

            if (backgroundPanel != null)
            {
                backgroundPanel.SetActive(!backgroundPanel.activeSelf);
            }
            else
            {
                Debug.LogWarning("Background panel does not have an Image component.");
            }
        }
        else
        {
            Debug.LogWarning("Background panel is not assigned.");
        }
    }
}
