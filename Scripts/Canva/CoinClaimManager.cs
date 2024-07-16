using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine;

public class CoinClaimManager : MonoBehaviour
{
    public GameObject[] coinUIObjects; // Array to hold the coin UI GameObjects
    public int[] coinScores; // Array to hold the score for each coin
    public Button[] claimButtons; // Array to hold the claim buttons for each coin
    public Button[] claimedButtons; // Array to hold the claimed buttons for each coin
    public TMP_Text timerText; // TextMeshPro element to display the timer
    public TMP_Text totalCoinsText; // TextMeshPro element to display the total coins

    public GameObject[] characters; // Array to hold the character GameObjects
    public int[] characterPrices; // Array to hold the price for each character
    public Button[] buyButtons; // Array to hold the buy buttons for each character
    public Button[] selectButtons; // Array to hold the select buttons for each character

    private int currentCoinIndex = 0;
    private DateTime nextClaimTime;
    private int totalCoins;

    void Start()
    {
        // Initialize each claim button with the corresponding coin index
        for (int i = 0; i < claimButtons.Length; i++)
        {
            int index = i;
            claimButtons[i].onClick.AddListener(() => ClaimCoin(index));
        }

        // Initialize the UI based on the saved data
        LoadClaimData();
        UpdateUI();
        UpdateTotalCoinsText();
        InitializeBuyButtons();
        InitializeSelectButtons();
    }

    void Update()
    {
        // Ensure timerText is not null before accessing it
        if (timerText != null)
        {
            // Update the timer display
            if (currentCoinIndex < coinUIObjects.Length)
            {
                TimeSpan remainingTime = nextClaimTime - DateTime.Now;
                if (remainingTime.TotalSeconds > 0)
                {
                    timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                    DisableClaimButtons();
                }
                else
                {
                    timerText.text = "00:00:00";
                    EnableClaimButtons();
                }
            }
            else
            {
                // When all coins are claimed, restart the scenario
                RestartScenario();
            }
        }
        else
        {
            Debug.LogWarning("timerText is not assigned in the Inspector.");
        }
    }

    void ClaimCoin(int index)
    {
        if (index != currentCoinIndex) return; // Ensure coins are claimed in sequence

        // Add the coin score to the total coins
        totalCoins += coinScores[index];
        UpdateTotalCoinsText();

        // Disable the claim button and enable the claimed button
        claimButtons[index].gameObject.SetActive(false);
        claimedButtons[index].gameObject.SetActive(true);

        // Move to the next coin UI object
        currentCoinIndex++;
        if (currentCoinIndex < coinUIObjects.Length)
        {
            nextClaimTime = DateTime.Now.AddHours(24);
        }
        else
        {
            // All coins claimed, so restart the scenario
            RestartScenario();
        }

        SaveClaimData();
    }

    void LoadClaimData()
    {
        currentCoinIndex = PlayerPrefs.GetInt("CurrentCoinIndex", 0);
        string nextClaimTimeString = PlayerPrefs.GetString("NextClaimTime", DateTime.MinValue.ToString());
        nextClaimTime = DateTime.Parse(nextClaimTimeString);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // Load character purchase data
        for (int i = 0; i < characters.Length; i++)
        {
            bool isPurchased = PlayerPrefs.GetInt("CharacterPurchased_" + i, 0) == 1;
            characters[i].SetActive(isPurchased);
            buyButtons[i].gameObject.SetActive(!isPurchased);
            selectButtons[i].gameObject.SetActive(isPurchased);
        }
    }

    void SaveClaimData()
    {
        PlayerPrefs.SetInt("CurrentCoinIndex", currentCoinIndex);
        PlayerPrefs.SetString("NextClaimTime", nextClaimTime.ToString());
        PlayerPrefs.SetInt("TotalCoins", totalCoins);

        // Save character purchase data
        for (int i = 0; i < characters.Length; i++)
        {
            bool isPurchased = !buyButtons[i].gameObject.activeSelf;
            PlayerPrefs.SetInt("CharacterPurchased_" + i, isPurchased ? 1 : 0);
        }
    }

    void UpdateUI()
    {
        // Update UI for each coin
        for (int i = 0; i < coinUIObjects.Length; i++)
        {
            coinUIObjects[i].SetActive(true);
            claimButtons[i].gameObject.SetActive(i == currentCoinIndex);
            claimedButtons[i].gameObject.SetActive(i < currentCoinIndex);
        }

        // If all coins are claimed, disable the claim button
        if (currentCoinIndex >= coinUIObjects.Length)
        {
            DisableClaimButtons();
            timerText.text = "All coins claimed!";
        }
    }

    void UpdateTotalCoinsText()
    {
        totalCoinsText.text = totalCoins.ToString() + "$";
    }

    void InitializeBuyButtons()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            int index = i;
            buyButtons[i].onClick.AddListener(() => BuyCharacter(index));
        }
    }

    void InitializeSelectButtons()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            int index = i;
            selectButtons[i].onClick.AddListener(() => SelectCharacter(index));
        }
    }

    void BuyCharacter(int index)
    {
        if (totalCoins >= characterPrices[index])
        {
            totalCoins -= characterPrices[index];
            UpdateTotalCoinsText();

            characters[index].SetActive(true);
            buyButtons[index].gameObject.SetActive(false); // Disable the buy button
            selectButtons[index].gameObject.SetActive(true); // Enable the select button

            // Save state after purchase
            SaveClaimData();
        }
        else
        {
            Debug.Log("Not enough coins to buy character " + index);
        }
    }

    void SelectCharacter(int index)
    {
        // Implement the logic to select a character
        Debug.Log("Character " + index + " selected");
    }

    void RestartScenario()
    {
        currentCoinIndex = 0;
        nextClaimTime = DateTime.Now; // Reset the timer so the first coin can be claimed immediately

        // Re-enable the claim buttons
        EnableClaimButtons();

        // Update UI
        UpdateUI();

        // Save the reset data
        SaveClaimData();
    }

    void DisableClaimButtons()
    {
        foreach (Button button in claimButtons)
        {
            button.interactable = false;
        }
    }

    void EnableClaimButtons()
    {
        if (currentCoinIndex < coinUIObjects.Length)
        {
            claimButtons[currentCoinIndex].interactable = true;
        }
    }
}
