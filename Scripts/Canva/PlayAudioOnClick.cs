using UnityEngine;
using UnityEngine.UI;

public class PlayAudioOnClick : MonoBehaviour
{
    public Button[] playButtons; // Reference to the Buttons in the Inspector
    public AudioClip audioClip; // Reference to the AudioClip in the Inspector
    public Slider volumeSlider; // Reference to the UI Slider for volume control
    public AudioSource audioSource; // The AudioSource that will play the clip

    private const string VolumePrefKey = "VolumeLevel1"; // Key for PlayerPrefs

    void Start()
    {
        // Add an AudioSource component to the GameObject this script is attached to
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        // Load the saved volume level from PlayerPrefs, default to 1 (max volume) if not found
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);

        // Set the slider value to the saved volume level
        volumeSlider.value = savedVolume;

        // Set the audio source volume to the saved volume level
        audioSource.volume = savedVolume;

        // Add listener for slider value change
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Iterate over each button in the playButtons array
        foreach (Button button in playButtons)
        {
            // Add a listener to the button to call the PlayAudio method when clicked
            button.onClick.AddListener(PlayAudio);
        }
    }

    void PlayAudio()
    {
        // Play the audio clip
        if (audioClip != null && audioSource != null)
        {
            audioSource.Play();
        }
        else if (audioClip == null)
        {
            Debug.LogWarning("No AudioClip assigned.");
        }
        else if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing.");
        }
    }

    public void SetVolume(float value)
    {
        // Check if the audio source is not null before setting the volume
        if (audioSource != null)
        {
            // Set the audio source volume to the slider value
            audioSource.volume = value;

            // Save the new volume level to PlayerPrefs
            PlayerPrefs.SetFloat(VolumePrefKey, value);
        }
        else
        {
            Debug.LogWarning("AudioSource is missing.");
        }
    }
}
