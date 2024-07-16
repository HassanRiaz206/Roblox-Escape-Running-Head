using UnityEngine;
using UnityEngine.UI;

public class DisableFunctionality : MonoBehaviour
{
    public Button disableButton; // Reference to the UI Button
    public MonoBehaviour scriptToDisable; // Reference to the script you want to disable

    void Start()
    {
        // Add listener to the button to call DisableScript method when pressed
        disableButton.onClick.AddListener(DisableScript);
    }

    void DisableScript()
    {
        // Disable the specified script
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = false;
        }
    }

    void OnDestroy()
    {
        // Remove listener when the script is destroyed
        disableButton.onClick.RemoveListener(DisableScript);
    }
}
