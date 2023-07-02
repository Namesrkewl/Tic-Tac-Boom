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
    public int turnCounter, currentTurn, round, playerMoveCount, opponentMoveCount, playerMoveMax, opponentMoveMax, stage;
    public bool playerVictory, opponentVictory, storyModeRunning, updatingGrid;
    public int[] playerBombCooldowns, opponentBombCooldowns;
    public bool bombInUse;
    public bool[] gridModification;
    public int turnBombUsed, gridSize, newGridSize, playerBombCount, opponentBombCount;
    public bool usingSmallBomb, usingCrossBomb, usingXBomb, usingMine;
    public string player, opponent;
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
    public Talents talents;
    public PlayMove playerMove;
    public Replay replay;
    public BuildGrid buildGrid;
    public LoadScene loadScene;
    public StoryModeAI storyModeAI;
    public ChangePlayers changePlayers;

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

    public IEnumerator StorySequence() {
        GameObject audio = GameObject.Find("BackgroundAudio");
        if (audio.GetComponent<AudioSource>().clip != loadingTheme) {
            audio.GetComponent<AudioSource>().clip = loadingTheme;
            audio.GetComponent<AudioSource>().Play();
        }
        GameObject ui = GameObject.Find("UI");
        ui.GetComponent<CanvasGroup>().alpha = 0;
        GameObject cover = GameObject.Find("Intro");
        GameObject fog = cover.transform.GetChild(0).gameObject;
        fog.SetActive(true);
        GameObject text = cover.transform.GetChild(1).gameObject;
        text.SetActive(true);
        GameObject value = text.transform.GetChild(0).gameObject;
        SetText(0);
        yield return new WaitForSeconds(2f);
        LeanTween.value(value, 0, 100, 3f).setOnUpdate(SetText);
        yield return new WaitForSeconds(4f);
        audio.GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(1f);
        text.SetActive(false);
        LeanTween.alpha(fog.GetComponent<Image>().rectTransform, 0, 2f);
        audio.GetComponent<AudioSource>().clip = windSound;
        audio.GetComponent<AudioSource>().PlayOneShot(windSound);
        yield return new WaitForSeconds(3f);
        fog.SetActive(false);
        ui.GetComponent<CanvasGroup>().alpha = 1;
        audio.GetComponent<AudioSource>().clip = battleTheme;
        audio.GetComponent<AudioSource>().Play();
    }

    private void SetText(float value) {
        GameObject.Find("Value").GetComponent<TextMeshProUGUI>().text = (value.ToString("F0") + "%");
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        loadScene = GameObject.Find("SceneManager").GetComponent<LoadScene>();
        if (scene.name == "PlayerVSAI" || scene.name == "LocalPVP") {
            LoadGame();
            BombCooldowns();
            buildGrid.BuildTheGrid();
            SetSprites();
            StartCoroutine(Game());
        } else if (scene.name == "StoryMode") {
            LoadGame();
            BombCooldowns();
            changePlayers.SetStoryModePlayers();
            SetSprites();
            buildGrid.BuildTheGrid();
            StartCoroutine(StoryMode());
        }
    }

    void UpdateVictory() {
        if (GameOver()) {
            if (playerVictory == true) {
                PlayerVictory();
            } else {
                OpponentVictory();
            }
        }
    }

    void SetTurn() {
        if (turnCounter % 2 == 1) {
            isPlayerTurn = true;    
            turnDisplay.transform.GetChild(1).gameObject.SetActive(false);
            turnDisplay.transform.GetChild(0).gameObject.SetActive(true);

            if (turnCounter != currentTurn) {
                for (int i = 0; i < playerBombCooldowns.Length; i++) {
                    if (playerBombCooldowns[i] > 0) {
                        playerBombCooldowns[i]--;
                    }
                }
                currentTurn = turnCounter;
            }
        } else {
            isPlayerTurn = false;
            turnDisplay.transform.GetChild(0).gameObject.SetActive(false);
            turnDisplay.transform.GetChild(1).gameObject.SetActive(true);

            if (turnCounter != currentTurn) {
                for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                    if (opponentBombCooldowns[i] > 0) {
                        opponentBombCooldowns[i]--;
                    }
                }
                currentTurn = turnCounter;
            }
        }
        round = (currentTurn / 2) + 1;
    }


    void SetCooldownText() {
        if (skills.activeSelf) {
            if (turnBombUsed != turnCounter) {
                if (isPlayerTurn) {
                    for (int i = 0; i < playerBombCooldowns.Length; i++) {
                        if (playerBombCooldowns[i] > 0) {
                            skills.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerBombCooldowns[i] + " Turn Cooldown";
                        } else {
                            skills.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
                        }
                    }
                } else {
                    for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                        if (opponentBombCooldowns[i] > 0) {
                            skills.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = opponentBombCooldowns[i] + " Turn Cooldown";
                        } else {
                            skills.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = skills.transform.GetChild(i).name;
                        }
                    }
                }
            } else {
                if (isPlayerTurn) {
                    for (int i = 0; i < playerBombCooldowns.Length; i++) {
                        skills.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bomb Used";
                    }
                } else {
                    for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                        skills.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bomb Used";
                    }
                }
            }
        }
    }
    public bool GameOver() {
        if (SceneManager.GetActiveScene().name == "Main") {
            return true;
        }
        int playerSpacesWon;
        int opponentSpacesWon;

        if (updatingGrid) {
            return false;
        }
        // Player Win Conditions

        // Vertical Win Condition
        for(int x = 0; x < gridSize; x++) {
            playerSpacesWon = 0;
            for (int y = 0; y < gridSize; y++) {
                string tile = x + "," + y;
                if (GameObject.Find(tile).CompareTag("Player")) {
                    playerSpacesWon += 1;
                    if (playerSpacesWon == gridSize) {
                        playerVictory = true;
                        break;
                    } else {
                        continue;
                    }
                } else {
                    break;
                }
            }
        }

        // Horizontal win Condition
        for (int y = 0; y < gridSize; y++) {
            playerSpacesWon = 0;
            for (int x = 0; x < gridSize; x++) {
                string tile = x + "," + y;
                if (GameObject.Find(tile).CompareTag("Player")) {
                    playerSpacesWon += 1;
                    if (playerSpacesWon == gridSize) {
                        playerVictory = true;
                        break;
                    } else {
                        continue;
                    }
                } else {
                    break;
                }
            }
        }

        // Left-Right Diagonal Win Condition
        playerSpacesWon = 0;
        for (int x = 0; x < gridSize; x++) {
            int y = x;

            string tile = x + "," + y;
            if (GameObject.Find(tile).CompareTag("Player")) {
                playerSpacesWon += 1;
                if (playerSpacesWon == gridSize) {
                    playerVictory = true;
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }

        // Right-Left Diagonal Win Condition
        playerSpacesWon = 0;
        for (int x = gridSize - 1; x >= 0; x--) {
            int y = (gridSize - 1) - x;

            string tile = x + "," + y;
            if (GameObject.Find(tile).CompareTag("Player")) {
                playerSpacesWon += 1;
                if (playerSpacesWon == gridSize) {
                    playerVictory = true;
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }


        // Opponent Win Conditions

        // Vertical Win Condition
        for (int x = 0; x < gridSize; x++) {
            opponentSpacesWon = 0;
            for (int y = 0; y < gridSize; y++) {
                string tile = x + "," + y;
                if (GameObject.Find(tile).CompareTag("Opponent")) {
                    opponentSpacesWon += 1;
                    if (opponentSpacesWon == gridSize) {
                        opponentVictory = true;
                        break;
                    } else {
                        continue;
                    }
                } else {
                    break;
                }
            }
        }

        // Horizontal win Condition
        for (int y = 0; y < gridSize; y++) {
            opponentSpacesWon = 0;
            for (int x = 0; x < gridSize; x++) {
                string tile = x + "," + y;
                if (GameObject.Find(tile).CompareTag("Opponent")) {
                    opponentSpacesWon += 1;
                    if (opponentSpacesWon == gridSize) {
                        opponentVictory = true;
                        break;
                    } else {
                        continue;
                    }
                } else {
                    break;
                }
            }
        }

        // Left-Right Diagonal Win Condition
        opponentSpacesWon = 0;
        for (int x = 0; x < gridSize; x++) {
            int y = x;

            string tile = x + "," + y;
            if (GameObject.Find(tile).CompareTag("Opponent")) {
                opponentSpacesWon += 1;
                if (opponentSpacesWon == gridSize) {
                    opponentVictory = true;
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }

        // Right-Left Diagonal Win Condition
        opponentSpacesWon = 0;
        for (int x = gridSize - 1; x >= 0; x--) {
            int y = (gridSize - 1) - x;

            string tile = x + "," + y;
            if (GameObject.Find(tile).CompareTag("Opponent")) {
                opponentSpacesWon += 1;
                if (opponentSpacesWon == gridSize) {
                    opponentVictory = true;
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }

        if (playerVictory == true || opponentVictory == true) {
            return true;
        }

        return false;
    }
    void PlayerVictory() {
        playerVictoryMenu.transform.localPosition = new Vector3(0, 0, 0);
    }

    void OpponentVictory() {
        opponentVictoryMenu.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void QuitGame() {
        settings.SetActive(true);
        settingsButton.SetActive(false);
    }
    public void ResumeGame() {
        settings.SetActive(false);
        settingsButton.SetActive(true);
    }

    void LoadGame() {
        // Sets Game values
        if (gridSize > 7) {
            gridSize = 7;
        } else if (gridSize < 1) {
            gridSize = 1;
        }
        gameObject.GetComponent<Talents>().skillMenu = GameObject.Find("SkillMenu");
        gameObject.GetComponent<Talents>().confirmSkillMenu = GameObject.Find("ConfirmSkillMenu");
        interfaceMenus = GameObject.Find("InterfaceMenus");
        skills = GameObject.Find("Skills");
        useSkillButton = GameObject.Find("UseSkillButton");
        playerVictoryMenu = GameObject.Find("PlayerVictoryMenu");
        playerVictoryMenu.transform.localPosition = new Vector3(0, -3840, 0);
        opponentVictoryMenu = GameObject.Find("OpponentVictoryMenu");
        opponentVictoryMenu.transform.localPosition = new Vector3(0, -3840, 0);
        turnDisplay = GameObject.Find("TurnDisplay");
        settings = GameObject.Find("Settings");
        settingsButton = GameObject.Find("SettingsButton");
        buildGrid.grid = GameObject.Find("Grid");
        nextFight = GameObject.Find("NextFight");
        nextFight.SetActive(false);
        UI = GameObject.Find("UI");
        HUD = GameObject.Find("HUD");
        isPlayerTurn = true;
        stage = 1;
        turnCounter = 1;
        currentTurn = 0;
        playerMoveCount = playerMoveMax;
        opponentMoveCount = opponentMoveMax;
        playerVictory = false;
        opponentVictory = false;
        bombInUse = false;
        turnBombUsed = 0;
        newGridSize = gridSize;
        gridModification = new bool[2];
    }

    void BombCooldowns() {
        playerBombCooldowns = new int[playerBombCount];
        opponentBombCooldowns = new int[opponentBombCount];
        // Initial Cooldowns
        for (int i = 0; i < playerBombCooldowns.Length; i++) {
            playerBombCooldowns[i] = 3;
        }
        for(int i = 0; i < opponentBombCooldowns.Length; i++) {
            opponentBombCooldowns[i] = 2;          
        }        
    }
    void SetSprites() {
        playerSprite = sprites[0];
        opponentSprite = sprites[1];
    }

    IEnumerator Game() {
        while (!GameOver()) {
            SetTurn();
            SetCooldownText();
            if (newGridSize > 0 && newGridSize < 8 && newGridSize != gridSize) {
                buildGrid.UpdateGrid(gridSize, newGridSize, gridModification);
            }
            yield return null;
        }
        UpdateVictory();
        yield return null;
    }

    public IEnumerator StoryMode() {
        while (!GameOver()) {
            SetTurn();
            SetCooldownText();
            if (newGridSize > 0 && newGridSize < 8 && newGridSize != gridSize && gridSize > 0 && gridSize < 8) {
                buildGrid.UpdateGrid(gridSize, newGridSize, gridModification);
            }
            if (!GameManager.instance.isPlayerTurn && opponentMoveCount > 0) {
                playerMove.StartPlayerMove(storyModeAI.AIMove());
                if (GameManager.instance.opponentMoveCount > 0) {
                    yield return new WaitForSeconds(0.2f);
                }
            }
            yield return null;
        }
        if (GameOver() && SceneManager.GetActiveScene().name != "Main") {
            UpdateVictory();
        }
        yield return null;
    }
}
