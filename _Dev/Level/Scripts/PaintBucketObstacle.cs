using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaintBucketObstacle : ObstacleController
{
    [SerializeField] private Rigidbody[] rbs;
    [SerializeField] private float explosionPositionOffset = 1f;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float upwardsModifier = 5f; 
    [SerializeField] private float rotationForce = 5f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private ParticleSystem[] _effects;
    protected override void EffectAction()
    {
        Vector3 explosionPosition = Vector3.zero;
        explosionPosition.z = transform.position.z;
        explosionPosition += Vector3.back * explosionPositionOffset;
        foreach (var rb in rbs)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.transform.SetParent(null);
            rb.AddExplosionForce(explosionForce, explosionPosition, 20f, upwardsModifier, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * rotationForce);
        }

        foreach (var effect in _effects)
        {
            effect.Play();
        }
        StartCoroutine(DelayDestroy(lifeTime));
    }

    private IEnumerator DelayDestroy(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }

        foreach (var rb in rbs)
        {
            Destroy(rb.gameObject);
        }
    }
}
