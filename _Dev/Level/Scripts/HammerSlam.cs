using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSlam : MonoBehaviour
{
    [SerializeField] private ParticleSystem effect;

    public void PlayEffect()
    {
        effect.Play();
    }
}
