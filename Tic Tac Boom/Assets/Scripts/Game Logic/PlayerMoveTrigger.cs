using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTrigger : MonoBehaviour
{
    // Trigger player movement at the current game object when mouse is clicked
    private void OnMouseUpAsButton() {
        EventManager.StartPlayerMoveEvent(gameObject);
    }
}
