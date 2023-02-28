using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Small_Bomb_Sounds : MonoBehaviour
{
    public AudioSource smallBombSounds;
    public AudioClip mouseOverSound;
    public AudioClip onClickSound;
    public AudioClip activeSound;
    public AudioClip onUseSound;
    public AudioClip detonateSound;

    public void MouseOverSound() {
        smallBombSounds.PlayOneShot(mouseOverSound);
    }

    public void OnClickSound() {
        smallBombSounds.PlayOneShot(onClickSound);
    }

    public void ActiveSound() {
        smallBombSounds.PlayOneShot(activeSound);
    }

    public void OnUseSound() {
        smallBombSounds.PlayOneShot(onUseSound);
    }

    public void DetonateSound() {
        smallBombSounds.PlayOneShot(detonateSound);
    }
}
