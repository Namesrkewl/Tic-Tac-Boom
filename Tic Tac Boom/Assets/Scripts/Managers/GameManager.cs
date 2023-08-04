using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager instance;
    public bool isPlayerTurn;
    public int turnCounter, currentTurn, round, playerMoveCount, enemyMoveCount, playerMoveMax, opponentMoveMax, stage;
    public bool playerVictory, opponentVictory, storyModeRunning, updatingGrid;
    public int[] playerBombCooldowns, opponentBombCooldowns;
    public bool bombInUse;
    public bool[] gridModification;
    public int turnBombUsed, gridSize, newGridSize, playerBombCount, opponentBombCount;
    public bool usingSmallBomb, usingCrossBomb, usingXBomb, usingMine;
    public string player, opponent;
    private bool loading;
    public Sprite[] sprites;
    public Sprite playerSprite, opponentSprite, mineSprite;
    [SerializeField] private AudioClip windSound;
    [SerializeField] private AudioClip battleTheme;
    [SerializeField] private AudioClip loadingTheme;
    [field: SerializeField] public AudioClip MainTheme { get; private set; }
    public AudioClip moveSound, bombSound;
    [field: SerializeField] public int Gold { get; private set; }
    [field: SerializeField]  public int Gems { get; private set; }

    // Scripts
    public Replay replay;
    public BuildGrid buildGrid;
    public LoadScene loadScene;
    public StoryModeAI storyModeAI;
    public PlayerManager playerManager;
    public ChangePlayers changePlayers;
    public TalentChoices talentChoices;
    public StoryManager storyManager;

    // Audio Clips
    [SerializeField] private AudioClip loadingSound;

    // Game Objects
    public GameObject skills;
    public GameObject useSkillMenu;
    public GameObject useSkillButton;
    public GameObject cancelSkillButton;
    public GameObject playerVictoryMenu;
    public GameObject opponentVictoryMenu;
    public GameObject interfaceMenus;
    public GameObject turnDisplay;
    public GameObject settings;
    public GameObject settingsButton;
    public GameObject nextFight;
    public GameObject HUD;
    public GameObject UI;
    

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
