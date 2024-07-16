using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static string targetSceneName; // Static variable to hold the target scene name

    private void Start()
    {
        // Start the coroutine to load the target scene after a delay
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Load the target scene
        SceneManager.LoadScene(targetSceneName);
    }
}