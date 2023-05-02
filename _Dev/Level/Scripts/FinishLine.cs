using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private GameObject finisherCamera;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        EventManager.AddListener<GameOverEvent>(OnGameOver);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
    }

    private void OnGameOver(GameOverEvent obj)
    {
        if (obj.IsWin)
        {
            _collider.enabled = false;
            finisherCamera.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EventManager.Broadcast(GameEventsHandler.PlayerFinishCrossedEvent);
    }
}
