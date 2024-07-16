using UnityEngine;
using System.Collections;

public class DeactivateObjectOnCollision : MonoBehaviour
{
    public GameObject objectToDeactivate; // Reference to the GameObject to deactivate
    public Color newColor = Color.red; // New color for the platform
    public Animator buttonAnimator; // Reference to the Animator component

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the specified GameObject to inactive
            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false);
            }
            else
            {
                Debug.LogWarning("No GameObject assigned to deactivate.");
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
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
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
