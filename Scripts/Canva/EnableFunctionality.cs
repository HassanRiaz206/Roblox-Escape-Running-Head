using UnityEngine;
using UnityEngine.UI;

public class EnableFunctionality : MonoBehaviour
{
    public Button enableButton; // Reference to the UI Button
    public MonoBehaviour scriptToEnable; // Reference to the script you want to enable

    void Start()
    {
        // Add listener to the button to call EnableScript method when pressed
        enableButton.onClick.AddListener(EnableScript);
    }

    void EnableScript()
    {
        // Enable the specified script
        if (scriptToEnable != null)
        {
            scriptToEnable.enabled = true;
        }
    }

    void OnDestroy()
    {
        // Remove listener when the script is destroyed
        enableButton.onClick.RemoveListener(EnableScript);
    }
}
