using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource backgroundMusic, soundEffects, UISounds;
    public List<AudioClip> musicClips;
    public List<AudioClip> soundEffectClips;
    public List<AudioClip> UISoundClips;
    public List<AudioClip> moveSounds;
    public List<AudioClip> bombSounds;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
