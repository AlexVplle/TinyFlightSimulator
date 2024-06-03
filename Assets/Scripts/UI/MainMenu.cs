using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MainMenu : MonoBehaviour
{
    public void PlaySolo()
    {
        NetworkManager.Singleton.Shutdown();
    }

    public void PlayMultiplayer()
    {
        // Display multiplayer options (Host, Client, Server)
        // You can show/hide additional buttons or UI elements here
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
}
