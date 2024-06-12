using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using SDD.Events;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject soloButton;
    [SerializeField] private GameObject multiplayerButton;
    [SerializeField] private GameObject clientButton;
    [SerializeField] private GameObject hostButton;

    public void Update()
    {
        if (Input.GetButtonDown("Submit"))
            PlaySolo();
    }
    public void PlaySolo()
    {
        EventManager.Instance.Raise(new SoloPlayerSpawnEvent { });
        EventManager.Instance.Raise(new DesactivateMainMenuEvent { });
    }

    public void PlayMultiplayer()
    {
        soloButton.SetActive(false);
        multiplayerButton.SetActive(false);
        clientButton.SetActive(true);
        hostButton.SetActive(true);
    }

    public void StartHost()
    {
        EventManager.Instance.Raise(new SoloPlayerSpawnEvent { });
        EventManager.Instance.Raise(new DesactivateMainMenuEvent { });
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        EventManager.Instance.Raise(new SoloPlayerSpawnEvent { });
        EventManager.Instance.Raise(new DesactivateMainMenuEvent { });
        NetworkManager.Singleton.StartClient();
    }
}
