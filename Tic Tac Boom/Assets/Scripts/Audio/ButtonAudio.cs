using System.Collections;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField] private AudioSource sounds;
    [SerializeField] private GameObject element;

    public void OnClickSound() {
        if (element.GetComponent<TweenAnimation>()) {
            if (!element.GetComponent<TweenAnimation>().IsMoving) {
                sounds.PlayOneShot(sounds.clip);
            }
        }
    }
}