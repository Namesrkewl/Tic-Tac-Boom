using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager instance;
    [field: SerializeField] public int Gold { get; private set; }
    [field: SerializeField]  public int Gems { get; private set; }

    public Tileset tileset;
    public enum Tileset {
        Basic,
        Alternate
    }
    public State state;
    public enum State {
        Idle,
        StoryMode
    }

    // Scripts
    public Replay replay;
    

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "PlayerVSAI" || scene.name == "LocalPVP") {
            // Respective Functions go here
        } else if (scene.name == "StoryMode") {
            state = State.StoryMode;
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update() {
        switch (state) {
            case State.StoryMode:
                StoryManager.instance.NewGame();
                state = State.Idle;
                break;
            default:
                break;
        }
    }
}
