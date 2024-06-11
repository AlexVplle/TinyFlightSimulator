using UnityEngine;
using SDD.Events;

public class HUDManager : MonoBehaviour, IEventHandler
{

    private static HUDManager m_Instance;
    public static HUDManager Instance => m_Instance;

    [SerializeField] private Canvas MainMenu;

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
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.AddListener<DesactivateMainMenuEvent>(DesactivateMainMenu);
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
}
