using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager instance;
    public int turn, round, stage, gridSize, newGridSize, minGridSize, maxGridSize;
    public List<Talent> unlockedSkills, unlockedPassives;
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
    

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "PlayerVSAI" || scene.name == "LocalPVP") {
            // Respective Functions go here
        } else if (scene.name == "StoryMode") {
            state = State.StoryMode;
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update() {
        switch (state) {
            case State.StoryMode:
                StartCoroutine(StoryManager.instance.NewGame());
                state = State.Idle;
                break;
            default:
                break;
        }
    }

    public void ClearTalents() {
        unlockedSkills = new List<Talent>();
        unlockedPassives = new List<Talent>();
    }

    public void ResetTalents() {
        unlockedSkills = new List<Talent>();
        unlockedPassives = new List<Talent>();
        unlockedSkills.Add(new Talent(Talent.TalentName.SmallBomb));
        unlockedSkills.Add(new Talent(Talent.TalentName.CrossBomb));
        unlockedSkills.Add(new Talent(Talent.TalentName.XBomb));
        unlockedSkills.Add(new Talent(Talent.TalentName.Mine));
        unlockedSkills.Add(new Talent(Talent.TalentName.BuildTiles));
        unlockedSkills.Add(new Talent(Talent.TalentName.DestroyTiles));
    }
    
}
