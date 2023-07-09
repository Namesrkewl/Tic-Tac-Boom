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
    public Talents talents;
    public PlayMove playerMove;
    public Replay replay;
    public BuildGrid buildGrid;
    public LoadScene loadScene;
    public StoryModeAI storyModeAI;
    public ChangePlayers changePlayers;
    public TalentChoices talentChoices;

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

    public IEnumerator StorySequence() {
        GameObject ui = GameObject.Find("UI");
        ui.GetComponent<CanvasGroup>().alpha = 0;
        GameObject HUD = GameObject.Find("HUD");
        GameObject turnIndicator = HUD.transform.GetChild(0).gameObject;
        turnIndicator.SetActive(false);
        GameObject playerIndicator = HUD.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        playerIndicator.SetActive(false);
        GameObject opponentIndicator = HUD.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        opponentIndicator.SetActive(false);
        GameObject nextPlayer = HUD.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        nextPlayer.transform.localPosition = new Vector3(2000, 0);
        nextPlayer.SetActive(true);
        GameObject nextOpponent = HUD.transform.GetChild(2).GetChild(0).GetChild(1).gameObject;
        nextOpponent.transform.localPosition = new Vector3(-2000, 0);
        nextOpponent.SetActive(true);
        GameObject nextFight = GameObject.Find("NextFight");
        GameObject cover = nextFight.transform.GetChild(0).gameObject;
        cover.SetActive(true);
        GameObject fog = nextFight.transform.GetChild(1).gameObject;
        GameObject background = nextFight.transform.GetChild(2).gameObject;
        background.SetActive(true);
        GameObject clouds = nextFight.transform.GetChild(4).gameObject;
        clouds.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameObject rollingFog = nextFight.transform.GetChild(3).gameObject;
        rollingFog.SetActive(true);
        GameObject audio = GameObject.Find("BackgroundAudio");
        audio.GetComponent<AudioSource>().clip = windSound;
        audio.GetComponent<AudioSource>().PlayOneShot(windSound);
        yield return new WaitForSeconds(0.5f);
        LeanTween.alpha(background, 0, 1f);
        yield return new WaitForSeconds(1f);
        background.SetActive(false);
        audio.GetComponent<AudioSource>().Stop();
        if (audio.GetComponent<AudioSource>().clip != loadingTheme) {
            audio.GetComponent<AudioSource>().clip = loadingTheme;
            audio.GetComponent<AudioSource>().Play();
        }
        LeanTween.alpha(clouds, 0, 0.1f);
        yield return new WaitForSeconds(1f);
        GameObject darkVeil = rollingFog.transform.GetChild(1).gameObject;
        LeanTween.alpha(darkVeil, 0, 0.1f);
        yield return new WaitForSeconds(0.1f);
        darkVeil.SetActive(true);
        LeanTween.alpha(darkVeil, 0.8f, 0.5f);
        yield return new WaitForSeconds(2f);
        clouds.SetActive(true);
        LeanTween.alpha(clouds, 1, 0.2f);
        yield return new WaitForSeconds(0.5f);
        GameObject vs = clouds.transform.GetChild(0).gameObject;
        vs.transform.localScale = Vector3.zero;
        vs.SetActive(true);
        LeanTween.moveLocal(nextPlayer, Vector3.zero, 0.1f).setEaseOutQuint();
        LeanTween.moveLocal(nextOpponent, Vector3.zero, 0.1f).setEaseOutQuint();
        LeanTween.scale(vs, Vector3.one, 0.5f).setEaseOutElastic();
        yield return new WaitForSeconds(0.1f);
        LeanTween.moveLocal(nextPlayer, new Vector3(-50, 0, 0), 5f);
        LeanTween.moveLocal(nextOpponent, new Vector3(50, 0, 0), 5f);
        yield return new WaitForSeconds(3f);
        LeanTween.alpha(clouds, 0, 0.5f);
        LeanTween.alphaCanvas(nextPlayer.GetComponent<CanvasGroup>(), 0, 0.5f);
        LeanTween.alphaCanvas(nextOpponent.GetComponent<CanvasGroup>(), 0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        nextPlayer.SetActive(false);
        nextOpponent.SetActive(false);
        clouds.SetActive(false);
        fog.SetActive(false);
        rollingFog.GetComponent<ParticleSystem>().Stop();
        audio.GetComponent<AudioSource>().Stop();
        audio.GetComponent<AudioSource>().clip = windSound;
        audio.GetComponent<AudioSource>().PlayOneShot(windSound);
        LeanTween.alpha(darkVeil, 0, 1f);
        yield return new WaitForSeconds(2f);
        darkVeil.SetActive(false);
        audio.GetComponent<AudioSource>().Stop();
        audio.GetComponent<AudioSource>().clip = battleTheme;
        audio.GetComponent<AudioSource>().Play();
        cover.SetActive(false);
        playerIndicator.SetActive(true);
        opponentIndicator.SetActive(true);
        turnIndicator.SetActive(true);
        ui.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(1f);
        loading = false;
    }

    public IEnumerator StageClear() {
        yield return new WaitForSeconds(2f);
        GameObject ui = GameObject.Find("UI");
        ui.GetComponent<CanvasGroup>().alpha = 0;
        GameObject HUD = GameObject.Find("HUD");
        GameObject turnIndicator = HUD.transform.GetChild(0).gameObject;
        turnIndicator.SetActive(false);
        GameObject playerIndicator = HUD.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        playerIndicator.SetActive(false);
        GameObject opponentIndicator = HUD.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        opponentIndicator.SetActive(false);
        GameObject nextFight = GameObject.Find("NextFight");
        GameObject rollingFog = nextFight.transform.GetChild(3).gameObject;
        rollingFog.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
        GameObject levelClear = GameObject.Find("LevelClearMenu");
        talentChoices.ClearTalentChoices();
        talentChoices.GenerateSkills(3);
        levelClear.transform.localPosition = Vector3.zero;
        levelClear.transform.GetChild(0).gameObject.SetActive(true);

    }

    public IEnumerator NextFight() {
        yield return new WaitForSeconds(2f);
        GameObject ui = GameObject.Find("UI");
        GameObject HUD = GameObject.Find("HUD");
        //GameObject turnIndicator = HUD.transform.GetChild(0).gameObject;
        //GameObject playerIndicator = HUD.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        //GameObject opponentIndicator = HUD.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        GameObject nextFight = GameObject.Find("NextFight");
        GameObject rollingFog = nextFight.transform.GetChild(3).gameObject;
        rollingFog.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
        GameObject levelClear = GameObject.Find("LevelClearMenu");
        talentChoices.ClearTalentChoices();

        levelClear.transform.localPosition = new Vector3(0, -3840);
        levelClear.transform.GetChild(0).gameObject.SetActive(true);

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
            loading = true;
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
        if (loading) {
            return;
        }
        if (turnCounter % 2 == 1) {
            if (player == "REBEL") {
                isPlayerTurn = true;
            } else if (player == "KING") {
                isPlayerTurn = false;
            } 
        } else {
            if (player == "REBEL") {
                isPlayerTurn = false;
            } else if (player == "KING") {
                isPlayerTurn = true;
            }
        }
        if (turnCounter != currentTurn) {
            if (isPlayerTurn) {
                playerMoveCount = playerMoveMax;
                for (int i = 0; i < playerBombCooldowns.Length; i++) {
                    if (playerBombCooldowns[i] > 0) {
                        playerBombCooldowns[i]--;
                    }
                }
            } else {
                opponentMoveCount = opponentMoveMax;
                playerMoveCount = playerMoveMax;
                for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                    if (opponentBombCooldowns[i] > 0) {
                        opponentBombCooldowns[i]--;
                    }
                }
            }
            currentTurn = turnCounter;
            turnDisplay.transform.GetChild(0).gameObject.SetActive(isPlayerTurn);
            turnDisplay.transform.GetChild(1).gameObject.SetActive(!isPlayerTurn);
            round = (currentTurn / 2) + 1;
        }
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
        if (SceneManager.GetActiveScene().name == "StoryMode" && stage < 15) {
            StartCoroutine(StageClear());
        } else {
            playerVictoryMenu.transform.localPosition = Vector3.zero;
        }
    }

    void OpponentVictory() {
        opponentVictoryMenu.transform.localPosition = Vector3.zero;
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
        UI = GameObject.Find("UI");
        HUD = GameObject.Find("HUD");
        isPlayerTurn = true;
        stage = 1;
        turnCounter = 1;
        currentTurn = 0;
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
        if (player == "REBEL") {
            playerSprite = sprites[0];
            opponentSprite = sprites[1];
        } else if (player == "KING") {
            playerSprite = sprites[1];
            opponentSprite = sprites[0];
        }
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
            if (!isPlayerTurn && opponentMoveCount > 0) {
                playerMove.StartPlayerMove(storyModeAI.AIMove());
                if (opponentMoveCount > 0) {
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
