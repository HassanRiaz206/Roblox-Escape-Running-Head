using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the UI Slider
    public AudioSource audioSource; // Reference to the Audio Source

    private const string VolumePrefKey = "VolumeLevel"; // Key for PlayerPrefs

    void Start()
    {
        // Load the saved volume level from PlayerPrefs, default to 1 (max volume) if not found
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);

        // Set the slider value to the saved volume level
        volumeSlider.value = savedVolume;

        // Set the audio source volume to the saved volume level
        audioSource.volume = savedVolume;

        // Add listener for slider value change
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        // Set the audio source volume to the slider value
        audioSource.volume = value;

        // Save the new volume level to PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }
}
