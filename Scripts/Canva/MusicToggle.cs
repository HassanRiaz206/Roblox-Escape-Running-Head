using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioSource gameAudioSource; // Reference to the AudioSource component

    private bool isAudioEnabled = true; // Flag to track if audio is enabled

    void Start()
    {
        // Initialize audio state based on PlayerPrefs (if saved)
        isAudioEnabled = PlayerPrefs.GetInt("AudioEnabled", 1) == 1;

        // Set initial audio state
        SetAudioState(isAudioEnabled);
    }

    public void ToggleAudio()
    {
        // Toggle audio state
        isAudioEnabled = !isAudioEnabled;
        SetAudioState(isAudioEnabled);
    }

    void SetAudioState(bool isEnabled)
    {
        // Enable or disable audio based on the flag
        gameAudioSource.enabled = isEnabled;

        // Save audio state to PlayerPrefs
        PlayerPrefs.SetInt("AudioEnabled", isEnabled ? 1 : 0);
        PlayerPrefs.Save();

        // Log current audio state
        Debug.Log("Audio is " + (isEnabled ? "enabled" : "disabled"));
    }
}
