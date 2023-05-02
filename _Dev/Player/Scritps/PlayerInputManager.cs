using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private Camera _mainCamera;
    private float _newTouchViewportX;
    private float _oldTouchViewportX;

    public float TouchDelta { get; private set; }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public float GetTouchDelta()
    {
        float delta = _newTouchViewportX - _oldTouchViewportX;
        _oldTouchViewportX = _newTouchViewportX;
        return delta;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _oldTouchViewportX = _mainCamera.ScreenToViewportPoint(Input.mousePosition).x;
        }
        if (Input.GetMouseButton(0))
        {
             _newTouchViewportX = _mainCamera.ScreenToViewportPoint(Input.mousePosition).x;
             TouchDelta = (_newTouchViewportX - _oldTouchViewportX) * Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            TouchDelta = 0;
        }
    }
}
