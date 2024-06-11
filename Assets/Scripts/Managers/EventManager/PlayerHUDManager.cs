using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using SDD.Events;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerHUDManager: MonoBehaviour, IEventHandler
{

    private static PlayerHUDManager m_Instance;
    public static PlayerHUDManager Instance => m_Instance;
    
    [SerializeField] private Canvas playerHUD;
    [SerializeField] private TextMeshProUGUI scoreRef;
    private int _score;

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
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<CreateSmokeEvent>(OnCreateSmoke);
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
