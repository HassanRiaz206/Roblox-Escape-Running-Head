using UnityEngine;

public class NXTPRV : MonoBehaviour
{
    public GameObject[] uiElements; // Array of UI elements to navigate through
    private int currentIndex = 0; // Index of the currently active UI element
    private bool isTransitioning = false; // Flag to prevent rapid button clicks

    void OnEnable()
    {
        // Ensure only the first UI element is active at start and others are disabled
        InitializeUIElements();
    }

    public void NextButtonClicked()
    {
        if (isTransitioning)
            return;

        StartCoroutine(TransitionToNext());
    }

    public void PreviousButtonClicked()
    {
        if (isTransitioning)
            return;

        StartCoroutine(TransitionToPrevious());
    }

    private void InitializeUIElements()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(i == 0);
        }
        currentIndex = 0;
        Debug.Log("UI Elements initialized. Only the first element is active.");
    }

    private System.Collections.IEnumerator TransitionToNext()
    {
        isTransitioning = true;

        // Hide the current UI element
        Debug.Log("Next button clicked. Current index: " + currentIndex);
        HideElement(currentIndex);

        // Move to the next UI element
        currentIndex++;
        if (currentIndex >= uiElements.Length)
        {
            currentIndex = 0; // Loop back to the first element
        }

        // Wait a frame to ensure the UI element is fully disabled
        yield return null;

        // Show the next UI element
        ShowElement(currentIndex);
        Debug.Log("Next element shown. New index: " + currentIndex);

        isTransitioning = false;
    }

    private System.Collections.IEnumerator TransitionToPrevious()
    {
        isTransitioning = true;

        // Hide the current UI element
        Debug.Log("Previous button clicked. Current index: " + currentIndex);
        HideElement(currentIndex);

        // Move to the previous UI element
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = uiElements.Length - 1; // Loop back to the last element
        }

        // Wait a frame to ensure the UI element is fully disabled
        yield return null;

        // Show the previous UI element
        ShowElement(currentIndex);
        Debug.Log("Previous element shown. New index: " + currentIndex);

        isTransitioning = false;
    }

    private void ShowElement(int index)
    {
        if (index >= 0 && index < uiElements.Length)
        {
            uiElements[index].SetActive(true);
            Debug.Log("Element " + index + " is now active.");
        }
        else
        {
            Debug.LogWarning("ShowElement: Index out of range.");
        }
    }

    private void HideElement(int index)
    {
        if (index >= 0 && index < uiElements.Length)
        {
            uiElements[index].SetActive(false);
            Debug.Log("Element " + index + " is now inactive.");
        }
        else
        {
            Debug.LogWarning("HideElement: Index out of range.");
        }
    }
}
