using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTrigger : MonoBehaviour
{
    private void OnMouseUpAsButton() {
        EventManager.StartPlayerMoveEvent(gameObject);
    }
}
