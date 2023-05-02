using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInput : MonoBehaviour
{
    [SerializeField] private InputField speedXInputField;
    [SerializeField] private InputField speedZInputField;
    [SerializeField] private InputField boundXInputField;
    [SerializeField] private Button startButton;
    private float _speedX;
    private float _speedZ;
    private float _boundX;
    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);

        _speedX = PlayerPrefs.GetFloat(PlayerPrefsStrings.DebugSpeedX, 10f);
        _speedZ = PlayerPrefs.GetFloat(PlayerPrefsStrings.DebugSpeedZ, 7.5f);
        _boundX = PlayerPrefs.GetFloat(PlayerPrefsStrings.DebugBoundX, 2.5f);

        speedXInputField.text = _speedX.ToString();
        speedZInputField.text = _speedZ.ToString();
        boundXInputField.text = _boundX.ToString();
    }

    private void OnStartButtonClick()
    {
        var evt = GameEventsHandler.DebugCallEvent;
        Single.TryParse(speedXInputField.text, out _speedX);
        Single.TryParse(speedZInputField.text, out _speedZ);
        Single.TryParse(boundXInputField.text, out _boundX);
        evt.BoundX = _boundX;
        evt.SpeedX = _speedX;
        evt.SpeedZ = _speedZ;
        PlayerPrefs.SetFloat(PlayerPrefsStrings.DebugSpeedX, _speedX);
        PlayerPrefs.SetFloat(PlayerPrefsStrings.DebugSpeedZ,  _speedZ);
        PlayerPrefs.SetFloat(PlayerPrefsStrings.DebugBoundX, _boundX);
        PlayerPrefs.Save();
        EventManager.Broadcast(evt);
    }
}
