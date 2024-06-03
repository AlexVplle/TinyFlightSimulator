using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using MFlight.Demo;

public class PlayerManager : MonoBehaviour, IEventHandler
{

    private static PlayerManager m_Instance;
    public static PlayerManager Instance => m_Instance;

    [SerializeField] private GameObject playerPrefab;

    private GameObject playerReference;

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
        GameObject playerReference = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        MFlight.Demo.Plane planeScript = playerReference.GetComponent<MFlight.Demo.Plane>();
        if (!planeScript)
        {
            Debug.Log("Plane Script not set");
            return;
        }
        planeScript.disableMultiplayer();
    }
}
