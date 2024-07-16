using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    public GameObject[] objectsToEnable; // Array of GameObjects to enable
    public float duration = 1.5f; // Duration in seconds for which the GameObjects will be enabled

    void Start()
    {
        // Ensure there are objects to enable
        if (objectsToEnable != null && objectsToEnable.Length > 0)
        {
            // Enable each object in the array at the start of the game
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }

            // Start coroutine to disable the objects after the specified duration
            StartCoroutine(DisableObjectsDelayed());
        }
        else
        {
            Debug.LogError("Objects to enable are not assigned or the array is empty!");
        }
    }

    IEnumerator DisableObjectsDelayed()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Disable each object in the array after the duration has passed
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }

        // Optionally, you can perform additional actions here after disabling the objects
    }
}
