using UnityEngine;
using SDD.Events;

public class HUDManager : MonoBehaviour, IEventHandler
{

    private static HUDManager m_Instance;
    public static HUDManager Instance => m_Instance;

    [SerializeField] private Canvas MainMenu;
    [SerializeField] private Canvas GameOver;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<DesactivateMainMenuEvent>(DesactivateMainMenu);
        EventManager.Instance.AddListener<KillPlayer>(ActivateGameOver);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<DesactivateMainMenuEvent>(DesactivateMainMenu);
        EventManager.Instance.RemoveListener<KillPlayer>(ActivateGameOver);
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void DesactivateMainMenu(DesactivateMainMenuEvent e)
    {
        MainMenu.gameObject.SetActive(false);
    }

    private void ActivateGameOver(KillPlayer e)
    {
        GameOver.gameObject.SetActive(true);
    }
}
