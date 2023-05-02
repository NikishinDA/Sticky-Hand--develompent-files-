using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHolderController : MonoBehaviour
{
    private Transform _playerTransform;
    private Animator _animator;
    [SerializeField] private float handPullTime = 1f;
    private bool _shakingOff;
    private void Awake()
    {
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);
    }

    private void Update()
    {
        if (_shakingOff)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _animator.SetTrigger("Tick");
                Taptic.Medium();
            }
        }
    }

    private void ShakeTick()
    {
        EventManager.Broadcast(GameEventsHandler.PlayerHandShakeTick);
    }
    private void OnFinisherEnd(FinisherEndEvent obj)
    {
        _shakingOff = false;
        _animator.SetTrigger("End");
    }

    private void OnGameOver(GameOverEvent obj)
    {
        if (obj.IsWin)
        {
            _playerTransform = obj.PlayerTransform;
            _playerTransform.SetParent(transform);
            StartCoroutine(PullHand(handPullTime));
        }
    }

    private IEnumerator PullHand(float time)
    {
        Vector3 startPosition = _playerTransform.localPosition;
        Vector3 endPosition = Vector3.zero;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            _playerTransform.localPosition = Vector3.Lerp(startPosition, endPosition, t / time);
            yield return null;
        }
        _playerTransform.localPosition = endPosition;
        _animator.SetTrigger("HandInPos");
    }

    private void OnPlayerOnPos()
    {
        _shakingOff = true;
        EventManager.Broadcast(GameEventsHandler.FinisherPlayerInPosition);
    }
}
