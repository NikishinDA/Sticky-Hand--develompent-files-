using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HandJewelryController : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    public JewelryType type;
    private Rigidbody _rb;
    private Collider[] _colliders;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();
    }

    public void JewelryPop()
    {
        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.AddExplosionForce(
            30f,
            transform.position + Vector3.down + 5* Vector3.back +  Vector3.left,
            20f,
            0,
            ForceMode.Impulse);
        _rb.AddTorque(Random.insideUnitSphere * 5, ForceMode.Impulse);
        StartCoroutine(DelayDestroy(lifeTime));
    }

    private IEnumerator DelayDestroy(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    public void JewelryDrop()
    {
        transform.SetParent(null);
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.AddForce(Vector3.down * 5, ForceMode.Impulse);
        foreach (var col in _colliders)
        {
            col.enabled = true;
        }
        var evt = GameEventsHandler.FinisherJewelryDroppedEvent;
        evt.Type = type;
        EventManager.Broadcast(evt);
    }
}
