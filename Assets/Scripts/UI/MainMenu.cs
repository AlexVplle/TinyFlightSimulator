using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using SDD.Events;

public class MainMenu : MonoBehaviour
{
    public void PlaySolo()
    {
        EventManager.Instance.Raise(new SoloPlayerSpawnEvent { });
        EventManager.Instance.Raise(new DesactivateMainMenuEvent { });
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
