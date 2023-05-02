using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandModelController : MonoBehaviour
{
    [SerializeField] private GameObject mHand;
    [SerializeField] private GameObject wHand;
    [SerializeField] private GameObject nailSGameObject;
    [SerializeField] private Material[] upgradeMats;
    [SerializeField] private Material[] nailMats;
    private Renderer _wRenderer;
    private Renderer _nailRenderer;
    private int upgradeLevel;
    [SerializeField] private ParticleSystem _upgradeEffect;
    private void Awake()
    {
        _wRenderer = wHand.GetComponent<SkinnedMeshRenderer>();
        _nailRenderer = nailSGameObject.GetComponent<SkinnedMeshRenderer>();
        EventManager.AddListener<PlayerHandUpgradeEvent>(OnUpgradeEvent);
        EventManager.AddListener<PlayerNailUpgradeEvent>(OnNailUpgradeEvent);
        SetUpgrade(PlayerPrefs.GetInt(PlayerPrefsStrings.UpgradeLevel, 0));
        SetNailUpgrade(PlayerPrefs.GetInt(PlayerPrefsStrings.NailUpgradeLevel, 0));
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerHandUpgradeEvent>(OnUpgradeEvent);
        EventManager.RemoveListener<PlayerNailUpgradeEvent>(OnNailUpgradeEvent);
    }

    private void OnNailUpgradeEvent(PlayerNailUpgradeEvent obj)
    {
        SetNailUpgrade(obj.UpgradeLevel);
        _upgradeEffect.Play();
        Taptic.Success();
    }

    private void SetNailUpgrade(int level)
    {
        if (level == 1)
        {
            var mats = _wRenderer.materials;
            mats[1] = nailMats[0];
            _wRenderer.materials = mats;
        }
        else if (level == 2)
        {
            var mats = _wRenderer.materials;
            mats[1] = nailMats[1];
            _wRenderer.materials = mats;
        }
        else if (level >= 3)
        {
            nailSGameObject.SetActive(true);
        }

        if (level == 4)
        {
            _nailRenderer.material = nailMats[0];
        }
        else if (level >= 5)
        {
            _nailRenderer.material = nailMats[1];
        }
    }

    private void OnUpgradeEvent(PlayerHandUpgradeEvent obj)
    {
        SetUpgrade(obj.UpgradeLevel);
        _upgradeEffect.Play();
        Taptic.Success();
    }

    private void SetUpgrade(int level)
    {
        /*if (level >= 1)
        {
            mHand.SetActive(false);
            wHand.SetActive(true);
        }*/
        /*if (level == 2)
        {
            var mats = _wRenderer.materials;
            mats[1] = nailMats[0];
            _wRenderer.materials = mats;
        }
        else if (level == 3)
        {
            var mats = _wRenderer.materials;
            mats[1] = nailMats[1];
            _wRenderer.materials = mats;
        }
        else if (level >= 4)
        {
            nailSGameObject.SetActive(true);
        }

        if (level == 5)
        {
            _nailRenderer.material = nailMats[0];
        }
        else if (level >= 6)
        {
            _nailRenderer.material = nailMats[1];
        }
*/
        if (level == 1)
        {
            _wRenderer.material = upgradeMats[0];
        }
        if (level == 2)
        {
            _wRenderer.material = upgradeMats[1];
        }
        else if (level == 3)
        {
            _wRenderer.material = upgradeMats[2];
        }
        else if (level == 4)
        {
            _wRenderer.material = upgradeMats[3];
        }
        
        else if (level >= 5)
        {
            _wRenderer.material = upgradeMats[4];
        }
    }
}
