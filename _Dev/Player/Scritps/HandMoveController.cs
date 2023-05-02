using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class HandMoveController : MonoBehaviour
{
    [SerializeField] private Transform moveTarget;
    [Header("X Movement")]
    [SerializeField] private float maxSpeedX;
    [SerializeField] private float movementBoundary;

    [Header("Z Movement")] [SerializeField]
    private float startSpeedZ = 10;

    [SerializeField] private float speedAddPerLevel = 0.5f;
    [SerializeField] private float maxSpeedZ = 20f;
    private float _speedZ;
   
    private float _newX;
    private Vector3 _newPosition;
    private PlayerInputManager _inputManager;
    private CharacterController _cc;
    private bool _move = false;
    [SerializeField] private float gravityForce;
    [SerializeField] private CinemachineVirtualCamera vCamera;
    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        _cc = GetComponent<CharacterController>();
        _newPosition = new Vector3();
        int level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1);
        _speedZ = startSpeedZ + speedAddPerLevel * (level - 1);
        if (_speedZ > maxSpeedZ)
        {
            _speedZ = maxSpeedZ;
        }
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<DebugCallEvent>(OnDebugCall);
        
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<DebugCallEvent>(OnDebugCall);
    }

    private void OnDebugCall(DebugCallEvent obj)
    {
        maxSpeedX = obj.SpeedX;
        movementBoundary = obj.BoundX;
        _speedZ = obj.SpeedZ;
    }

    private void OnGameOver(GameOverEvent obj)
    {
        if (obj.IsWin)
            _move = false;
        else
        {
            _speedZ = 0;
            vCamera.m_Follow = null;
        }
    }

    private void OnGameStart(GameStartEvent obj)
    {
        _move = true;
    }

    private void Update()
    {
        if (_move)
        {
            _newX = maxSpeedX * _inputManager.GetTouchDelta();
            if (Mathf.Abs(transform.position.x + _newX) >= movementBoundary)
            {
                _newX = 0;
            }

            _newPosition.x = _newX;
            _newPosition.y = - gravityForce * Time.deltaTime;
            _newPosition.z = _speedZ * Time.deltaTime;
            _cc.Move(_newPosition);
        }
    }
}
