using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EnableDisableGroup
{
    public GameObject enableObject; // The GameObject to be enabled
    public GameObject[] disableObjects; // The array of GameObjects to be disabled
    public Button selectButton; // The select button to trigger the action
    public string enableObjectKey; // Unique key for PlayerPrefs
    public string selectButtonKey; // Unique key for PlayerPrefs to save the button state
}

public class EnableDisableManager : MonoBehaviour
{
    public EnableDisableGroup[] enableDisableGroups; // Array of enable/disable groups

    void Start()
    {
        // Load saved states and set the initial state of objects
        foreach (EnableDisableGroup group in enableDisableGroups)
        {
            bool isEnabled = PlayerPrefs.GetInt(group.enableObjectKey, 0) == 1;
            group.enableObject.SetActive(isEnabled);

            bool isButtonInteractable = PlayerPrefs.GetInt(group.selectButtonKey, 1) == 1;
            group.selectButton.interactable = isButtonInteractable;

            if (isEnabled)
            {
                foreach (GameObject obj in group.disableObjects)
                {
                    obj.SetActive(false);
                }
            }

            // Add listeners to all select buttons
            group.selectButton.onClick.AddListener(() => OnSelectButtonPressed(group));
        }

        // Register scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSelectButtonPressed(EnableDisableGroup selectedGroup)
    {
        // Enable the specified GameObject
        selectedGroup.enableObject.SetActive(true);

        // Save the state of all groups in PlayerPrefs
        foreach (EnableDisableGroup group in enableDisableGroups)
        {
            bool isEnabled = (group == selectedGroup);
            PlayerPrefs.SetInt(group.enableObjectKey, isEnabled ? 1 : 0);
            PlayerPrefs.SetInt(group.selectButtonKey, group.selectButton.interactable ? 1 : 0);
        }
        PlayerPrefs.Save();

        // Disable the specified GameObjects and set button interactability
        foreach (EnableDisableGroup group in enableDisableGroups)
        {
            if (group != selectedGroup)
            {
                group.enableObject.SetActive(false);
            }

            group.selectButton.interactable = (group != selectedGroup);
            PlayerPrefs.SetInt(group.selectButtonKey, group.selectButton.interactable ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Load saved states and set the initial state of objects
        foreach (EnableDisableGroup group in enableDisableGroups)
        {
            bool isEnabled = PlayerPrefs.GetInt(group.enableObjectKey, 0) == 1;
            group.enableObject.SetActive(isEnabled);

            bool isButtonInteractable = PlayerPrefs.GetInt(group.selectButtonKey, 1) == 1;
            group.selectButton.interactable = isButtonInteractable;

            if (isEnabled)
            {
                foreach (GameObject obj in group.disableObjects)
                {
                    obj.SetActive(false);
                }
            }

            // Re-add listeners to all select buttons
            group.selectButton.onClick.AddListener(() => OnSelectButtonPressed(group));
        }
    }

    void OnDestroy()
    {
        // Unregister scene change event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
