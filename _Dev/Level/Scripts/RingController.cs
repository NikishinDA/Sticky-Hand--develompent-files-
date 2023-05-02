using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RingController : MonoBehaviour
{
    [SerializeField] private Material[] ringMaterials;
    [SerializeField] private Material[] gemsMaterials;
    [SerializeField] private Renderer gemsRenderer;
    private Renderer _ringRenderer;
    [HideInInspector] public bool isSilver;
    private void Awake()
    {
        _ringRenderer = GetComponent<MeshRenderer>();
        int mat = Random.Range(0, 2);
        _ringRenderer.material = ringMaterials[mat];
        isSilver = mat == 0;
        if (gemsRenderer)
            gemsRenderer.material = gemsMaterials[Random.Range(0, gemsMaterials.Length)];
    }
}
