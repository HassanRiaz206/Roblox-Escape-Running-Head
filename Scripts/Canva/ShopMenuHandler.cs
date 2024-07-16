using UnityEngine;

public class ShopMenuHandler : MonoBehaviour
{
    public GameObject shopPanel; // Reference to the shop panel GameObject
    public GameObject[] uiObjectsToDisable; // Array of UI objects to disable

    // Call this method when the Shop menu button is pressed
    public void OnShopMenuButtonPressed()
    {
        // Enable the shop panel
        shopPanel.SetActive(true);

        // Disable the other UI objects
        foreach (GameObject uiObject in uiObjectsToDisable)
        {
            if (uiObject != shopPanel) // Ensure we don't disable the shop panel itself
            {
                uiObject.SetActive(false);
            }
        }
    }
}
