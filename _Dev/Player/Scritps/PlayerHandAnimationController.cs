using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandAnimationController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        EventManager.AddListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);

    }

    private void OnFinisherEnd(FinisherEndEvent obj)
    {
        _animator.SetTrigger("ThumbUp");
    }

    private void OnGameOver(GameOverEvent obj)
    {
        _animator.SetTrigger("Relax");
    }

    private void OnPlayerTakeDamage(PlayerTakeDamageEvent obj)
    {
        switch (obj.ObstacleType)
        {
            case ObstacleType.Saw:
                _animator.SetTrigger("Hurt");
                break;
            case ObstacleType.Mud:
            case ObstacleType.Paint:
                _animator.SetTrigger("Paint");
                break;
        }
        
        Taptic.Failure();
    }

    private void ThrowOffJewelry()
    {
        EventManager.Broadcast(GameEventsHandler.JewelryThrowEvent);
    }
}
