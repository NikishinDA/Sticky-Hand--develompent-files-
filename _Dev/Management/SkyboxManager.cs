using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    [SerializeField] private Material[] skyboxMaterials;
    [SerializeField] private Color[] fogColors;

    private void Awake()
    {
        int level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1) - 1;
        RenderSettings.skybox = skyboxMaterials[level / 5 % 3];
        RenderSettings.fogColor = fogColors[level / 5 % 3];
    }
}
