using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    public AudioSource sounds;
    public GameObject element;

    public void OnClickSound() {
        if (element.GetComponent<MoveMenu>()) {
            if (!element.GetComponent<MoveMenu>().isMoving) {
                sounds.PlayOneShot(sounds.clip);
            }
        } else if (element.GetComponent<PopUpAnimation>()) {
            if (!element.GetComponent<PopUpAnimation>().isMoving) {
                sounds.PlayOneShot(sounds.clip);
            }
        }
    }
}