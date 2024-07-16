using UnityEngine;
using TMPro; // Ensure TextMeshPro namespace is included
using UnityEngine.SceneManagement;

public class GateCollisionHandler : MonoBehaviour
{
    public string loadingSceneName = "Loading"; // The name of the loading scene
    public string targetSceneName; // The name of the target scene to load after the loading scene
    public int requiredScore; // The required score to pass through this gate
    public TMP_Text scoreText; // Reference to the TextMeshPro text element in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Read current score from TMP text element
            int currentScore = GetCurrentScore();

            // Check if the player has enough score to pass through the gate
            if (currentScore >= requiredScore)
            {
                // Store the target scene name in a static variable to access in the loading scene
                SceneTransitionManager.targetSceneName = targetSceneName;
                // Load the loading scene
                SceneManager.LoadScene(loadingSceneName);
            }
            else
            {
                Debug.Log("Insufficient score to pass through the gate.");
                // Optionally, you can provide feedback to the player here
            }
        }
    }

    // Method to safely retrieve the current score from scoreText
    private int GetCurrentScore()
    {
        string text = scoreText.text.Trim();
        int score;

        // Check if the format is "Score: 123"
        if (text.StartsWith("Score:"))
        {
            string[] parts = text.Split(':');
            if (parts.Length > 1 && int.TryParse(parts[1].Trim(), out score))
            {
                return score;
            }
            else
            {
                Debug.LogWarning($"Failed to parse score from '{text}'. Score format might be incorrect.");
            }
        }
        // If the format is just "123"
        else if (int.TryParse(text, out score))
        {
            return score;
        }
        else
        {
            Debug.LogWarning($"Score text format is incorrect: '{text}'. Expected format is 'Score: <number>' or '<number>'.");
        }

        return 0; // Return 0 if score parsing fails
    }
}
