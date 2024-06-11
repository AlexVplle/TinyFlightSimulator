using UnityEngine;
using SDD.Events;

public class MissileManager: MonoBehaviour, IEventHandler
{

    private static MissileManager m_Instance;
    public static MissileManager Instance => m_Instance;
    [SerializeField] private ParticleSystem[] _particleSystemPrefabs;
    [SerializeField] private GameObject _waterMissilePrefab;
    [SerializeField] private GameObject _smokePrefab;

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
        EventManager.Instance.AddListener<CreateSmokeEvent>(OnCreateSmoke);
        EventManager.Instance.AddListener<DestroyFireballEvent>(OnDestroyFireball);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<CreateNewMissileEvent>(OnCreateMissile);
        EventManager.Instance.RemoveListener<DestroyMissileEvent>(OnDestoryMissile);
        EventManager.Instance.RemoveListener<CreateSmokeEvent>(OnCreateSmoke);
        EventManager.Instance.RemoveListener<DestroyFireballEvent>(OnDestroyFireball);
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

        Instantiate(_waterMissilePrefab, e.Transform.position, e.Transform.rotation);
    }

    public void OnCreateSmoke(CreateSmokeEvent e)
    {
        Instantiate(_smokePrefab, e.Position, _smokePrefab.transform.rotation);
    }

    public void OnDestroyFireball(DestroyFireballEvent e)
    {
       Destroy(e.fireball); 
    }

    public void OnDestoryMissile(DestroyMissileEvent e)
    {
        Destroy(e.Missile);
    }
}
