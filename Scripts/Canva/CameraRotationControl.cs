using UnityEngine;
using UnityEngine.UI;

public class CameraRotationControl : MonoBehaviour
{
    public CameraRotation cameraRotationScript; // Reference to the CameraRotation script
    public Slider rotationSpeedSlider; // Reference to the UI Slider

    private const string RotationSpeedPrefKey = "RotationSpeed"; // Key for PlayerPrefs

    void Start()
    {
        // Load the saved rotation speed from PlayerPrefs, default to 0.1f if not found
        float savedRotationSpeed = PlayerPrefs.GetFloat(RotationSpeedPrefKey, 0.4f);

        // Set the slider value to the saved rotation speed
        rotationSpeedSlider.value = savedRotationSpeed;

        // Set the camera rotation speed to the saved value
        cameraRotationScript.rotationSpeed = savedRotationSpeed;

        // Add listener for slider value change
        rotationSpeedSlider.onValueChanged.AddListener(SetRotationSpeed);
    }

    public void SetRotationSpeed(float value)
    {
        // Set the camera rotation speed to the slider value
        cameraRotationScript.rotationSpeed = value;

        // Save the new rotation speed to PlayerPrefs
        PlayerPrefs.SetFloat(RotationSpeedPrefKey, value);
    }

    void OnDestroy()
    {
        // Remove listener when the script is destroyed
        rotationSpeedSlider.onValueChanged.RemoveListener(SetRotationSpeed);
    }
}
