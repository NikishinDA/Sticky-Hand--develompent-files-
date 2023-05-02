using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryController : MonoBehaviour
{
    [SerializeField] protected JewelryType type;
    [SerializeField] private RingController ringController;
    public JewelryType Type => type;

    protected void OnTriggerEnter(Collider other)
    {
        var evt = GameEventsHandler.ItemCollectEvent;
        evt.Type = type;
        if (ringController)
        {
            evt.IsSilver = ringController.isSilver;
            float offset = transform.position.x - other.transform.position.x;
            evt.RingOffset = offset;
        }

        EventManager.Broadcast(evt);
        Destroy(gameObject);
    }
}
