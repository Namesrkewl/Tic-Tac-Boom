using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemsManager : MonoBehaviour
{
    public static ParticleSystemsManager instance;
    public List<ParticleSystem> particleSystems;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
