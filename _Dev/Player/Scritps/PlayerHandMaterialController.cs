using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandMaterialController : MonoBehaviour
{
    private Renderer _renderer;
    [SerializeField] private Material mudMat;
    [SerializeField] private Material paintMat;
    private Material _startMat;
    private void Awake()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
        EventManager.AddListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<JewelryThrowEvent>(OnJewelryThrow);
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.RemoveListener<JewelryThrowEvent>(OnJewelryThrow);
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);

    }

    private void OnGameStart(GameStartEvent obj)
    {
        _startMat = _renderer.material;
    }

    private void OnJewelryThrow(JewelryThrowEvent obj)
    {
        _renderer.material = _startMat;
    }

    private void OnPlayerTakeDamage(PlayerTakeDamageEvent obj)
    {
        switch (obj.ObstacleType)
        {
            case ObstacleType.Mud:
            {
                _renderer.material = mudMat;
            }
                break;
            case ObstacleType.Paint:
            {
                _renderer.material = paintMat;
            }
                break;
        }
    }
}
