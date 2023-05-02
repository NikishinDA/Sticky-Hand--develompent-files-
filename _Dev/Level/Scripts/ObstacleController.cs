using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Saw,
    Paint,
    Mud
}

public class ObstacleController : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private ObstacleType type;
    [SerializeField] private ParticleSystem _effect;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var evt = GameEventsHandler.PlayerTakeDamageEvent;
        evt.ObstacleType = type;
        EventManager.Broadcast(evt);
        _collider.enabled = false;
        EffectAction();
    }

    protected virtual void EffectAction()
    {
        if (_effect)
        {
            _effect.Play();
        }
    }
}
