using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinAudioPlayer : MonoBehaviour
{
    public AudioClip coinSound; // Audio clip for the coin sound

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider that triggered is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered coin trigger!");

            // Play the coin sound using a temporary GameObject
            if (coinSound != null)
            {
                PlaySound(coinSound);
                Debug.Log("Coin sound played!");
            }
            else
            {
                Debug.LogError("Coin sound clip is not assigned!");
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        // Create a temporary GameObject to play the sound
        GameObject tempAudioSource = new GameObject("TempAudio");
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();

        // Configure the AudioSource
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.Play();

        // Destroy the temporary GameObject after the sound has finished playing
        Destroy(tempAudioSource, clip.length);
    }
}
