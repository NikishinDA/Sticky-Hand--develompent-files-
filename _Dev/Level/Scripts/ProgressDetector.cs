using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventManager.Broadcast(GameEventsHandler.PlayerProgressEvent);
    }
}
