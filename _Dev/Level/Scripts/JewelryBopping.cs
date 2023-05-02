using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JewelryBopping : MonoBehaviour
{
    [SerializeField] private AnimationCurve _yPosition;
    private float _time;

    private void Update()
    {
        var transformPosition = transform.position;
        transformPosition.y = _yPosition.Evaluate(_time);
        transform.position = transformPosition;
        _time += Time.deltaTime;
    }
}
