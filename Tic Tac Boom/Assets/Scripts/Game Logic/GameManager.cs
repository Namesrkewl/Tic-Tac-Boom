using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager instance;
    public bool isPlayerTurn, isGameOver;
    public int turnCounter, currentTurn;
    private bool playerVictory, opponentVictory;
    public int[][] playerBombCooldowns, opponentBombCooldowns;
    public bool bombInUse;
    public bool[] gridModification;
    public int turnBombUsed, gridSize, newGridSize, playerBombCount, opponentBombCount;
    public Sprite[] sprites;
    public Sprite playerSprite, opponentSprite;

    // Scripts
    public Talents talents;
    public PlayMove playerMove;
    public Replay replay;
    public BuildGrid buildGrid;
    public LoadScene loadScene;

    // Game Objects
    public GameObject skillMenu;
    public GameObject useSkillMenu;
    public GameObject useSkillButton;
    public GameObject cancelSkillButton;
    public GameObject replayMenu;
    public GameObject replayButton;
    public GameObject playerVictoryMenu;
    public GameObject opponentVictoryMenu;
    public GameObject interfaceMenus;
    public GameObject turnDisplay;
    public GameObject HUD;
    public GameObject quitPopUp;
    public GameObject menuOverlay;
    public GameObject quitButton;
    

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
        if (scene.name == "StoryMode" || scene.name == "PlayerVSAI" || scene.name == "LocalPVP") {
            LoadGame();
            BombCooldowns();
            buildGrid.BuildTheGrid();
            SetSprites();
        }
    }
    private void Update() {
        if (SceneManager.GetActiveScene().name == "StoryMode" || SceneManager.GetActiveScene().name == "PlayerVSAI" || SceneManager.GetActiveScene().name == "LocalPVP") {
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
        }
    }

    void SetTurn() {
        if (turnCounter % 2 == 1) {
            isPlayerTurn = true;
            turnDisplay.transform.GetChild(0).GetComponent<Text>().text = "Player 1's Turn!";
            turnDisplay.GetComponent<Image>().color = new Color32(240, 170, 0, 255);
            
            if (turnCounter != currentTurn) {
                for (int i = 0; i < playerBombCooldowns.Length; i++) {
                    if (playerBombCooldowns[i][0] > 0) {
                        playerBombCooldowns[i][0]--;
                    }
                }
                currentTurn = turnCounter;
            }
        } else {
            isPlayerTurn = false;
            turnDisplay.transform.GetChild(0).GetComponent<Text>().text = "Player 2's Turn!";
            turnDisplay.GetComponent<Image>().color = new Color32(0, 111, 239, 255);
            if (turnCounter != currentTurn) {
                for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                    if (opponentBombCooldowns[i][0] > 0) {
                        opponentBombCooldowns[i][0]--;
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
                        if (playerBombCooldowns[i][0] > 0) {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = playerBombCooldowns[i][0] + " Turn Cooldown";
                        } else {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = skillMenu.transform.GetChild(i).name;
                        }
                    }
                } else {
                    for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                        if (opponentBombCooldowns[i][0] > 0) {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = opponentBombCooldowns[i][0] + " Turn Cooldown";
                        } else {
                            skillMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = skillMenu.transform.GetChild(i).name;
                        }
                    }
                }
            } else {
                if (isPlayerTurn) {
                    for (int i = 0; i < playerBombCooldowns.Length; i++) {
                        skillMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Bomb Used";
                    }
                } else {
                    for (int i = 0; i < opponentBombCooldowns.Length; i++) {
                        skillMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Bomb Used";
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
        HUD.SetActive(true);
        turnDisplay.SetActive(false);
        playerVictoryMenu.SetActive(true);
        interfaceMenus.SetActive(false);
        replayMenu.SetActive(true);
    }

    void OpponentVictory() {
        HUD.SetActive(true);
        turnDisplay.SetActive(false);
        opponentVictoryMenu.SetActive(true);
        interfaceMenus.SetActive(false);
        replayMenu.SetActive(true);
    }

    public void QuitGame() {
        menuOverlay.SetActive(true);
        quitPopUp.SetActive(true);
        quitButton.SetActive(false);
    }
    public void ResumeGame() {
        quitPopUp.SetActive(false);
        menuOverlay.SetActive(false);
        quitButton.SetActive(true);
    }

    void LoadGame() {
        // Sets Game values
        interfaceMenus = GameObject.Find("InterfaceMenus");
        skillMenu = GameObject.Find("SkillMenu");
        GameObject.Find("Small Bomb").GetComponent<Button>().onClick.AddListener(talents.SetUsingSmallBomb);
        GameObject.Find("Cross Bomb").GetComponent<Button>().onClick.AddListener(talents.SetUsingCrossBomb);
        GameObject.Find("X Bomb").GetComponent<Button>().onClick.AddListener(talents.SetUsingXBomb);
        GameObject.Find("Mine").GetComponent<Button>().onClick.AddListener(talents.SetUsingMine);
        GameObject.Find("BuildTiles").GetComponent<Button>().onClick.AddListener(playerMove.BuildTiles);
        GameObject.Find("DestroyTiles").GetComponent<Button>().onClick.AddListener(playerMove.DestroyTiles);
        skillMenu.SetActive(false);
        useSkillMenu = GameObject.Find("UseSkillMenu");
        useSkillMenu.SetActive(true);
        useSkillButton = GameObject.Find("UseSkillButton");
        useSkillButton.SetActive(true);
        useSkillButton.GetComponent<Button>().onClick.AddListener(talents.UseSkill);
        cancelSkillButton = GameObject.Find("CancelSkillButton");
        cancelSkillButton.GetComponent<Button>().onClick.AddListener(talents.CancelBombUse);
        cancelSkillButton.SetActive(false);
        replayMenu = GameObject.Find("ReplayMenu");
        replayButton = GameObject.Find("ReplayButton");
        replayButton.GetComponent<Button>().onClick.AddListener(replay.PlayAgain);
        replayMenu.SetActive(false);
        playerVictoryMenu = GameObject.Find("PlayerVictoryMenu");
        playerVictoryMenu.SetActive(false);
        opponentVictoryMenu = GameObject.Find("OpponentVictoryMenu");
        opponentVictoryMenu.SetActive(false);
        turnDisplay = GameObject.Find("TurnDisplay");
        turnDisplay.SetActive(true);
        HUD = GameObject.Find("HUD");
        HUD.SetActive(false);
        quitPopUp = GameObject.Find("QuitPopUp");
        GameObject.Find("QuitNo").GetComponent<Button>().onClick.AddListener(ResumeGame);
        GameObject.Find("QuitYes").GetComponent<Button>().onClick.AddListener(loadScene.LoadMain);
        quitPopUp.SetActive(false);
        menuOverlay = GameObject.Find("MenuOverlay");
        menuOverlay.SetActive(false);
        quitButton = GameObject.Find("QuitButton");
        quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        quitButton.SetActive(true);
        buildGrid.grid = GameObject.Find("Grid");
        isPlayerTurn = true;
        turnCounter = 1;
        currentTurn = 0;
        playerVictory = false;
        opponentVictory = false;
        bombInUse = false;
        turnBombUsed = 0;
        isGameOver = false;
        newGridSize = gridSize;
        gridModification = new bool[2];
    }

    void BombCooldowns() {
        playerBombCount = 4;
        opponentBombCount = 4;
        playerBombCooldowns = new int[playerBombCount][];
        opponentBombCooldowns = new int[opponentBombCount][];
        // Initial Cooldowns
        for (int i = 0; i < playerBombCooldowns.Length; i++) {
            playerBombCooldowns[i] = new int[2];
            playerBombCooldowns[i][0] = 3;
            if (i == 0) {
                playerBombCooldowns[i][1] = 1;
            } else {
                playerBombCooldowns[i][1] = 2;
            }
        }
        for(int i = 0; i < opponentBombCooldowns.Length; i++) {
            opponentBombCooldowns[i] = new int[2];
            opponentBombCooldowns[i][0] = 2;
            if (i == 0) {
                opponentBombCooldowns[i][1] = 1;
            } else {
                opponentBombCooldowns[i][1] = 2;
            }            
        }        
    }
    void SetSprites() {
        playerSprite = sprites[0];
        opponentSprite = sprites[1];
    }
}
