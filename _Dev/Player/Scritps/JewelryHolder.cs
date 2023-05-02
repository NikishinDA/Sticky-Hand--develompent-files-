using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryHolder : MonoBehaviour
{
    [HideInInspector] public HandJewelryController Jewelry;
    [HideInInspector] public int Finger;
    [SerializeField] private ParticleSystem _silverEffect;
    [SerializeField] private ParticleSystem _goldEffect;

    public void PlayEffect(bool isSilver = true)
    {
        if (isSilver)
        {
            if (_silverEffect)
            {
                _silverEffect.Play();
            }
        }
        else
        {
            if (_goldEffect)
            {
                _goldEffect.Play();
            }
        }
    }
}
