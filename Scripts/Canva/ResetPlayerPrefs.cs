using UnityEngine;
using UnityEngine.UI;

public class ResetPlayerPrefs : MonoBehaviour
{
    public GameObject[] childGameObjects; // Array of the 4 child GameObjects
    public GameObject character; // The character GameObject to enable or disable

    void Update()
    {
        if (AreAllChildrenDisabled())
        {
            character.SetActive(true); // Enable the character GameObject
        }
        else
        {
            character.SetActive(false); // Disable the character GameObject if any child is enabled
        }
    }
    bool AreAllChildrenDisabled()
    {
        // Check if all child GameObjects are disabled
        foreach (GameObject child in childGameObjects)
        {
            if (child.activeSelf)
            {
                return false; // Return false if any child GameObject is enabled
            }
        }
        return true; // Return true if all child GameObjects are disabled
    }
}
