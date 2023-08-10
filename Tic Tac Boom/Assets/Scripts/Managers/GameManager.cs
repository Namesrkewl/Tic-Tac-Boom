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

    // Scripts
    public Replay replay;
    public LoadScene loadScene;
    public StoryManager storyManager;
    

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        loadScene = GameObject.Find("SceneManager").GetComponent<LoadScene>();
        if (scene.name == "PlayerVSAI" || scene.name == "LocalPVP") {
            //LoadGame();
            //BombCooldowns();
            //StartCoroutine(Game());
        } else if (scene.name == "StoryMode") {
            //LoadGame();
            //BombCooldowns();
            storyManager.NewGame();
        }
    }
}
