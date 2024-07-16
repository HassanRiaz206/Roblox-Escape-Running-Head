using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ButtonManager : MonoBehaviour
{
    public Button buttonA; // Reference to Button A
    public Button buttonB; // Reference to Button B

    void Start()
    {
        // Disable Button A at the start of the game
        buttonA.gameObject.SetActive(false);

        // Optionally, ensure Button B starts as disabled or enabled as needed
        // buttonB.gameObject.SetActive(false);

        // Start checking the state of Button B
        StartCoroutine(CheckButtonBState());
    }

    IEnumerator CheckButtonBState()
    {
        // Keep checking until Button B is enabled
        while (!buttonB.gameObject.activeSelf)
        {
            yield return null; // Wait for the next frame
        }

        // Once Button B is enabled, enable Button A
        buttonA.gameObject.SetActive(true);
    }
}
