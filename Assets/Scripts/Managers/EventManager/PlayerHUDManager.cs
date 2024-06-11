using UnityEngine;
using SDD.Events;
using TMPro;
using UnityEngine.UI;
using IEventHandler = SDD.Events.IEventHandler;

public class PlayerHUDManager: MonoBehaviour, IEventHandler
{

    private static PlayerHUDManager m_Instance;
    public static PlayerHUDManager Instance => m_Instance;
    
    [SerializeField] private Canvas playerHUD;
    [SerializeField] private TextMeshProUGUI scoreRef;
    [SerializeField] private Image compass;
    
    private int _score;
    
    float GetYawFromQuaternion(Quaternion rotation)
    {
        Vector3 euler = rotation.eulerAngles;
        return euler.y;
    }
    
    void OnFlightRotationUpdate(FlightRotationUpdateEvent e)
    {
        Quaternion rotation = e.rotation;
        float rot = GetYawFromQuaternion(rotation);
        compass.rectTransform.localEulerAngles = new Vector3(0, 0, rot);
    }

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
        EventManager.Instance.AddListener<CreateSmokeEvent>(OnCreateSmoke);
        EventManager.Instance.AddListener<FlightRotationUpdateEvent>(OnFlightRotationUpdate);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<CreateSmokeEvent>(OnCreateSmoke);
        EventManager.Instance.RemoveListener<FlightRotationUpdateEvent>(OnFlightRotationUpdate);
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

    public void OnCreateSmoke(CreateSmokeEvent e)
    {
        _score += 10;
        scoreRef.text = "Score : " + _score;
    }
}
