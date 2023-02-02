using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHistory : MonoBehaviour
{
    public static SceneHistory instance;
    public List<string> sceneHistory = new();
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
