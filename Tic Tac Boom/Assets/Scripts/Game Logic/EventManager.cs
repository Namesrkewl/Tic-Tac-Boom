using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<GameObject> PlayerMoveEvent;

    public static void StartPlayerMoveEvent(GameObject go) {
        PlayerMoveEvent?.Invoke(go);
    }
}
