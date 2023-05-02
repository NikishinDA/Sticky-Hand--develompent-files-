using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRingController : HandJewelryController
{
    [SerializeField] private Renderer gemsRenderer;
    [SerializeField] private Material[] ringMaterials;
    [SerializeField] private Material[] gemsMaterials;
    private Renderer _ringRenderer;
    protected override void Awake()
    {
        base.Awake();
        _ringRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(bool isSilver)
    {
        _ringRenderer.material = isSilver ? ringMaterials[1] : ringMaterials[0];
        if (gemsRenderer)
            gemsRenderer.material = gemsMaterials[Random.Range(0, gemsMaterials.Length)];
    }
}
