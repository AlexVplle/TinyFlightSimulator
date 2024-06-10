using UnityEngine;
using SDD.Events;

public class MissileManager: MonoBehaviour, IEventHandler
{

    private static MissileManager m_Instance;
    public static MissileManager Instance => m_Instance;
    [SerializeField] private ParticleSystem[] _particleSystemPrefabs;
    [SerializeField] private GameObject _waterMissilePrefab;

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
        EventManager.Instance.AddListener<CreateNewMissileEvent>(OnCreateMissile);
        EventManager.Instance.AddListener<DestroyMissileEvent>(OnDestoryMissile);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.AddListener<CreateNewMissileEvent>(OnCreateMissile);
        EventManager.Instance.AddListener<DestroyMissileEvent>(OnDestoryMissile);
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

    public void OnCreateMissile(CreateNewMissileEvent e)
    {
        foreach (ParticleSystem ps in _particleSystemPrefabs)
        {
            Instantiate(ps, e.Transform);
        }

        GameObject missile = Instantiate(_waterMissilePrefab, e.Transform.position, e.Transform.rotation);
        missile.GetComponent<Rigidbody>().velocity = e.Velocity;
    }

    public void OnDestoryMissile(DestroyMissileEvent e)
    {
        Destroy(e.Missile);
    }
}
