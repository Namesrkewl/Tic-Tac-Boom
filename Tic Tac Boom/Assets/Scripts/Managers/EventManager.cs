using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    // Events
    public static event Action<GameObject> PlayerMoveEvent;

    // Event Logic
    public static void StartPlayerMoveEvent(GameObject go) {
        PlayerMoveEvent?.Invoke(go);
    }
}
