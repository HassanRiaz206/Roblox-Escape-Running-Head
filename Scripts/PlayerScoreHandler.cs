using UnityEngine;
using TMPro;

public class PlayerScoreHandler : MonoBehaviour
{
    public TMP_Text scoreText;
    private int totalScore;

    void Start()
    {
        totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        UpdateScoreText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreObject"))
        {
            totalScore += 10;
            PlayerPrefs.SetInt("TotalScore", totalScore);
            UpdateScoreText();

            // Optionally, you can destroy or disable the collided object
            // Destroy(other.gameObject);
            // or
            // other.gameObject.SetActive(false);
        }
    }

    void UpdateScoreText()
    {
        scoreText.text =  totalScore.ToString();
    }
}
