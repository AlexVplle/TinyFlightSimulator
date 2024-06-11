using UnityEngine;
using SDD.Events;

public class PlayerManager : MonoBehaviour, IEventHandler
{

    private static PlayerManager m_Instance;
    public static PlayerManager Instance => m_Instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private EndlessTerrain endlessTerrain;

    public static GameObject playerReference;

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
        EventManager.Instance.AddListener<SoloPlayerSpawnEvent>(SetPlayerReference);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.AddListener<SoloPlayerSpawnEvent>(SetPlayerReference);
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

    private void SetPlayerReference(SoloPlayerSpawnEvent e)
    {
        if (!playerPrefab)
        {
            Debug.Log("Player Prefab not set");
            return;
        }
        playerReference = Instantiate(playerPrefab, new Vector3(0.0f, 100f, 0.0f), Quaternion.identity);
        playerReference.tag = "Plane";
        MFlight.Demo.Plane planeScript = playerReference.GetComponent<MFlight.Demo.Plane>();
        if (!planeScript)
        {
            Debug.Log("Plane Script not set");
            return;
        }
        endlessTerrain.viewer = playerReference.transform;
    }
}
