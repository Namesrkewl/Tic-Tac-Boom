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
    public int turnCounter, currentTurn;
    private bool playerVictory, opponentVictory;
    public int[] playerBombCooldowns, opponentBombCooldowns;
    public bool bombInUse;
    public bool[] gridModification;
    public int turnBombUsed, gridSize, newGridSize, playerBombCount, opponentBombCount;
    public Sprite[] sprites;
    public Sprite playerSprite, opponentSprite;
    [SerializeField] private AudioClip windSound;
    [SerializeField] private AudioClip battleTheme;
    [SerializeField] private AudioClip loadingTheme;
    [field: SerializeField] public AudioClip MainTheme { get; private set; }
    public AudioClip moveSound;
    [field: SerializeField] public int Gold { get; private set; }
    [field: SerializeField]  public int Gems { get; private set; }

    // Scripts
    public Talents talents;
    public PlayMove playerMove;
    public Replay replay;
    public BuildGrid buildGrid;
    public LoadScene loadScene;
    public StoryModeAI storyModeAI;

    // Game Objects
    public GameObject skillMenu;
    public GameObject useSkillMenu;
    public GameObject useSkillButton;
    public GameObject cancelSkillButton;
    public GameObject playerVictoryMenu;
    public GameObject opponentVictoryMenu;
    public GameObject interfaceMenus;
    public GameObject turnDisplay;
    public GameObject settings;
    public GameObject settingsButton;
    

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
        if (scene.name == "StoryMode" || scene.name == "PlayerVSAI" || scene.name == "LocalPVP") {
            LoadGame();
            BombCooldowns();
            buildGrid.BuildTheGrid();
            SetSprites();
        } 
        if (scene.name == "StoryMode") {
            StartCoroutine(StoryMode());
        }
    }
    private void Update() {
        if (SceneManager.GetActiveScene().name == "PlayerVSAI" || SceneManager.GetActiveScene().name == "LocalPVP") {
            SetTurn();
            SetCooldownText();
            if (newGridSize > 0 && newGridSize < 8 && newGridSize != gridSize) {
                buildGrid.UpdateGrid(gridSize, newGridSize, gridModification);
                gridSize = newGridSize;
            } else {
                if (GameOver()) {
                    if (playerVictory == true) {
                        PlayerVictory();
                    } else {
                        OpponentVictory();
                    }
                }
            }
        } else if (SceneManager.GetActiveScene().name == "StoryMode") {
            if (newGridSize > 0 && newGridSize < 8 && newGridSize != gridSize) {
                buildGrid.UpdateGrid(gridSize, newGridSize, gridModification);
                gridSize = newGridSize;
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
    }


    void SetCooldownText() {
        if (skillMenu.activeSelf) {
            if (turnBombUsed != turnCounter) {
                if (isPlayerTurn) {
                    for (int i = 0; i < playerBombCooldowns.Length; i++) {
                        if (playerBombCooldowns[i] > 0) {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerBombCooldowns[i] + " Turn Cooldown";
                        } else {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
                        }
                    }
                } else {
                    for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                        if (opponentBombCooldowns[i] > 0) {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = opponentBombCooldowns[i] + " Turn Cooldown";
                        } else {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillMenu.transform.GetChild(i).name;
                        }
                    }
                }
            } else {
                if (isPlayerTurn) {
                    for (int i = 0; i < playerBombCooldowns.Length; i++) {
                        skillMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bomb Used";
                    }
                } else {
                    for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                        skillMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bomb Used";
                    }
                }
            }
        }
    }
    bool GameOver() {

        int playerSpacesWon;
        int opponentSpacesWon;

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
        interfaceMenus = GameObject.Find("InterfaceMenus");
        //skillMenu = GameObject.Find("SkillMenu");
        useSkillButton = GameObject.Find("UseSkillButton");
        //cancelSkillButton = GameObject.Find("CancelSkillButton");
        //cancelSkillButton.GetComponent<Button>().onClick.AddListener(talents.CancelSkill);
        playerVictoryMenu = GameObject.Find("PlayerVictoryMenu");
        playerVictoryMenu.transform.localPosition = new Vector3(0, -3840, 0);
        opponentVictoryMenu = GameObject.Find("OpponentVictoryMenu");
        opponentVictoryMenu.transform.localPosition = new Vector3(0, -3840, 0);
        turnDisplay = GameObject.Find("TurnDisplay");
        settings = GameObject.Find("Settingss");
        settingsButton = GameObject.Find("SettingsButton");
        //settingsButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        buildGrid.grid = GameObject.Find("Grid");
        isPlayerTurn = true;
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
        playerSprite = sprites[0];
        opponentSprite = sprites[1];
    }

    IEnumerator StoryMode() {
        while (!GameOver()) {
            SetTurn();
            SetCooldownText();
            if (!GameManager.instance.isPlayerTurn) {
                playerMove.PlayerMove(storyModeAI.AIMove());
                yield return new WaitForSeconds(0.2f);
                playerMove.PlayerMove(storyModeAI.AIMove());
                yield return new WaitForSeconds(0.2f);
                playerMove.NextTurn();
            }
            yield return null;
        }
        if (GameOver()) {
            if (playerVictory == true) {
                PlayerVictory();
            } else {
                OpponentVictory();
            }
        }
        yield return null;
    }
}
