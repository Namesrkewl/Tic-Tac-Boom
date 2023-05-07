using UnityEngine;


public class BackgroundAudio : MonoBehaviour
{
    public static BackgroundAudio instance;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
