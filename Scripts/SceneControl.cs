using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

    // This method restarts the current scene
    public void RestartScene()
    {

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // This method teleports to the lobby scene
    public void TeleportToLobby()
    {
        // Load the lobby scene (replace "LobbySceneName" with the actual name of your lobby scene)
        SceneManager.LoadScene("Lobby");
    }
}
