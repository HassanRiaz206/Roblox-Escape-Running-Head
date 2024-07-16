using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerDeathHandler : MonoBehaviour
{
    public GameObject deathParticleEffect;
    public TMP_Text coinText;
    private Vector3 respawnPosition;
    private int totalCoins;

    // Variable to track if player is currently dead
    private bool isDead = false;

    // List of scripts that need to be disabled during respawn
    public MonoBehaviour[] scriptsToDisable;

    void Start()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateCoinText();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isDead)
        {
            isDead = true; // Set player to dead state to prevent multiple deaths

            // Disable the player GameObject
            gameObject.SetActive(false);

            PlayDeathEffect();
            // Disable other scripts
            SetScriptsEnabled(false);

            // Respawn the player with a delay
            Invoke("RespawnPlayer", .2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SaveZone"))
        {
            respawnPosition = other.transform.position;
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            totalCoins += 10;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            UpdateCoinText();

            Destroy(other.gameObject);
        }
    }

    void PlayDeathEffect()
    {
        // Instantiate the death particle effect at the player's position
        Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
    }

    void RespawnPlayer()
    {
        // Set the player position to the stored respawn position
        transform.position = respawnPosition;
        // Reactivate the player GameObject
        gameObject.SetActive(true);

        // Enable other scripts
        SetScriptsEnabled(true);

        isDead = false; // Reset the player's death state
    }

    void UpdateCoinText()
    {
        coinText.text = totalCoins.ToString() + "$";
    }

    // Function to enable/disable other scripts
    void SetScriptsEnabled(bool enabled)
    {
        foreach (var script in scriptsToDisable)
        {
            script.enabled = enabled;
        }
    }
}
