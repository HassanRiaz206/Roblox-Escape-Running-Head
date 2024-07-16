using UnityEngine;
using System.Collections;
public class ActivateObjectOnCollision : MonoBehaviour
{
    public GameObject objectToActivate; // Reference to the GameObject to activate
    public Color newColor = Color.red; // New color for the platform
    public Animator buttonAnimator; // Reference to the Animator component

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the specified GameObject to active
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No GameObject assigned to activate.");
            }

            // Play the button animation
            if (buttonAnimator != null)
            {
                buttonAnimator.Play("ButtonAnimation"); // Ensure the animation clip is named "ButtonAnimation"
                StartCoroutine(WaitForAnimation(buttonAnimator));
            }
            else
            {
                Debug.LogWarning("No Animator component assigned.");
                ChangePlatformColor();
            }
        }
    }

    private IEnumerator WaitForAnimation(Animator animator)
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        ChangePlatformColor();
    }

    private void ChangePlatformColor()
    {
        // Change the color of the platform child GameObject
        Transform platformTransform = transform.Find("platform");
        if (platformTransform != null)
        {
            Renderer platformRenderer = platformTransform.GetComponent<Renderer>();
            if (platformRenderer != null)
            {
                platformRenderer.material.color = newColor;
            }
            else
            {
                Debug.LogWarning("No Renderer component found on the platform GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("No child GameObject named 'platform' found.");
        }
    }
}
