using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    public int turn, round, stage, gridSize, newGridSize;
    public bool updatingGrid;
    public GameObject UI, HUD, nextFight, grid, turnDisplay, playerVictoryMenu, enemyVictoryMenu;
    public PlayerManager playerManager;
    public AudioManager audioManager;
    public PlayMove playerMove;
    public BuildGrid buildGrid;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        Debug.Log(playerManager.player.remainingMoves);
        Debug.Log(playerManager.player.maxMoves);
        Debug.Log(gameState);
    }

    public GameState gameState;
    public enum GameState {
        Loading,
        LevelStart,
        PlayerTurn,
        EnemyTurn,
        EndTurn,
        StageClear,
        NextFight,
        GridLocked,
        PlayerVictory,
        EnemyVictory,
        Draw
    }

    private IEnumerator StoryMode() {
        while (!GameOver()) {
            switch (gameState) {
                case GameState.EndTurn:
                    yield return StartCoroutine(EndTurn());
                    break;
                case GameState.EnemyTurn:
                    yield return StartCoroutine(EnemyTurn());
                    break;
                case GameState.GridLocked:
                    yield return StartCoroutine(GridLocked());
                    break;
                case GameState.PlayerTurn:
                    yield return StartCoroutine(PlayerTurn());
                    break;
                default:
                    break;
            }
            yield return null;
        }

        yield return null;
    }

    public void NewGame() {
        UI = GameObject.Find("UI");
        HUD = GameObject.Find("HUD");
        nextFight = GameObject.Find("NextFight");
        grid = GameObject.Find("Grid");
        turnDisplay = GameObject.Find("TurnDisplay");
        playerVictoryMenu = GameObject.Find("PlayerVictoryMenu");
        enemyVictoryMenu = GameObject.Find("EnemyVictoryMenu");
        playerManager.SetPlayers(true);
        playerManager.SetSkins();
        playerManager.player.maxMoves = 1;
        buildGrid.grid = grid;
        turn = 1;
        round = 1;
        stage = 1;
        gridSize = 3;
        newGridSize = 3;
        gameState = GameState.Loading;
        StartCoroutine(Loading());
    }

    private IEnumerator Loading() {
        playerManager.SetCharacterSprite(playerManager.player);
        playerManager.enemy.character = Player.Character.Vagabond;
        playerManager.SetCharacterSprite(playerManager.enemy);
        playerManager.SetAI(playerManager.enemy);
        newGridSize = playerManager.storyModeAI.startingGridSize;
        gridSize = newGridSize;
        buildGrid.BuildTheGrid(gridSize);
        UI.GetComponent<CanvasGroup>().alpha = 0;
        GameObject HUD = GameObject.Find("HUD");
        GameObject turnIndicator = HUD.transform.GetChild(0).gameObject;
        turnIndicator.SetActive(false);
        GameObject playerIndicator = HUD.transform.GetChild(1).GetChild(0).gameObject;
        playerIndicator.SetActive(false);
        GameObject enemyIndicator = HUD.transform.GetChild(2).GetChild(0).gameObject;
        enemyIndicator.SetActive(false);
        GameObject nextPlayer = HUD.transform.GetChild(1).GetChild(1).gameObject;
        nextPlayer.transform.localPosition = new Vector3(2000, 0);
        nextPlayer.SetActive(true);
        GameObject nextEnemy = HUD.transform.GetChild(2).GetChild(1).gameObject;
        nextEnemy.transform.localPosition = new Vector3(-2000, 0);
        nextEnemy.SetActive(true);
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
        audioManager.soundEffects.PlayOneShot(audioManager.soundEffectClips[0]);
        yield return new WaitForSeconds(0.5f);
        LeanTween.alpha(background, 0, 1f);
        yield return new WaitForSeconds(1f);
        background.SetActive(false);
        audioManager.soundEffects.Stop();
        if (audioManager.backgroundMusic.clip != audioManager.musicClips[0]) {
            audioManager.soundEffects.clip = audioManager.musicClips[0];
            audioManager.soundEffects.Play();
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
        LeanTween.moveLocal(nextEnemy, Vector3.zero, 0.1f).setEaseOutQuint();
        LeanTween.scale(vs, Vector3.one, 0.5f).setEaseOutElastic();
        yield return new WaitForSeconds(0.1f);
        LeanTween.moveLocal(nextPlayer, new Vector3(-50, 0, 0), 5f);
        LeanTween.moveLocal(nextEnemy, new Vector3(50, 0, 0), 5f);
        yield return new WaitForSeconds(3f);
        LeanTween.alpha(clouds, 0, 0.5f);
        LeanTween.alphaCanvas(nextPlayer.GetComponent<CanvasGroup>(), 0, 0.5f);
        LeanTween.alphaCanvas(nextEnemy.GetComponent<CanvasGroup>(), 0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        nextPlayer.SetActive(false);
        nextEnemy.SetActive(false);
        clouds.SetActive(false);
        fog.SetActive(false);
        rollingFog.GetComponent<ParticleSystem>().Stop();
        audioManager.soundEffects.Stop();
        audioManager.soundEffects.PlayOneShot(audioManager.soundEffectClips[0]);
        LeanTween.alpha(darkVeil, 0, 1f);
        yield return new WaitForSeconds(2f);
        darkVeil.SetActive(false);
        audioManager.backgroundMusic.Stop();
        audioManager.backgroundMusic.clip = audioManager.musicClips[1];
        audioManager.backgroundMusic.Play();
        cover.SetActive(false);
        playerIndicator.SetActive(true);
        enemyIndicator.SetActive(true);
        turnIndicator.SetActive(true);
        UI.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(1f);
        gameState = GameState.LevelStart;
        StartCoroutine(LevelStart());
        yield return null;
    }

    private IEnumerator LevelStart() {
        if (playerManager.player.faction == Player.Faction.Exiled) {
            gameState = GameState.PlayerTurn;
        } else {
            gameState = GameState.EnemyTurn;
        }
        StartCoroutine(StoryMode());
        yield return null;
    }

    private IEnumerator PlayerTurn() {
        if (playerManager.player.state == Player.State.Inactive) {
            playerManager.player.state = Player.State.Playing;
            playerManager.enemy.state = Player.State.Inactive;
            playerManager.player.remainingMoves = playerManager.player.maxMoves;
            turnDisplay.transform.GetChild(0).gameObject.SetActive(true);
            turnDisplay.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        if (playerManager.player.remainingMoves <= 0) {
            playerManager.player.state = Player.State.Idle;
            gameState = GameState.EndTurn;
        }

        yield return null;
    }

    private IEnumerator EnemyTurn() {
        if (playerManager.enemy.state == Player.State.Inactive) {
            playerManager.enemy.state = Player.State.Playing;
            playerManager.player.state = Player.State.Inactive;
            playerManager.enemy.remainingMoves = playerManager.enemy.maxMoves;
            turnDisplay.transform.GetChild(1).gameObject.SetActive(true);
            turnDisplay.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (playerManager.enemy.remainingMoves > 0) {
            playerManager.StartPlayerMove(playerManager.storyModeAI.AIMove());
            yield return new WaitForSeconds(0.2f);
        } else {
            playerManager.enemy.state = Player.State.Idle;
            gameState = GameState.EndTurn;
        }

        yield return null;
    }

    private IEnumerator EndTurn() {
        turn++;
        round = turn / 2;
        if (playerManager.player.state == Player.State.Idle) {
            gameState = GameState.EnemyTurn;
        } else if (playerManager.enemy.state == Player.State.Idle) {
            gameState = GameState.PlayerTurn;
        }
        yield return null;
    }

    private IEnumerator StageClear() {
        yield return new WaitForSeconds(2f);
        GameObject ui = GameObject.Find("UI");
        UI.GetComponent<CanvasGroup>().alpha = 0;
        GameObject HUD = GameObject.Find("HUD");
        GameObject turnIndicator = HUD.transform.GetChild(0).gameObject;
        turnIndicator.SetActive(false);
        GameObject playerIndicator = HUD.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        playerIndicator.SetActive(false);
        GameObject enemyIndicator = HUD.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        enemyIndicator.SetActive(false);
        GameObject nextFight = GameObject.Find("NextFight");
        GameObject rollingFog = nextFight.transform.GetChild(3).gameObject;
        rollingFog.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
        GameObject levelClear = GameObject.Find("LevelClearMenu");
        /*
        talentChoices.ClearTalentChoices();
        talentChoices.GenerateSkills(3);*/
        levelClear.transform.localPosition = Vector3.zero;
        levelClear.transform.GetChild(0).gameObject.SetActive(true);
        yield return null;
    }

    private IEnumerator NextFight() {
        yield return null;
    }

    private IEnumerator GridLocked() {
        yield return null;
    }

    private IEnumerator PlayerVictory() {
        if (stage < 15) {
            StartCoroutine(StageClear());
        } else {
            playerVictoryMenu.transform.localPosition = Vector3.zero;
        }
        yield return null;
    }

    private IEnumerator EnemyVictory() {
        enemyVictoryMenu.transform.localPosition = Vector3.zero;
        yield return null;
    }

    private IEnumerator Draw() {
        yield return null;
    }
    public bool GameOver() {
        int playerSpacesWon;
        int enemySpacesWon;

        if (updatingGrid) {
            return false;
        }
        // Player Win Conditions

        // Vertical Win Condition
        for (int x = 0; x < gridSize; x++) {
            playerSpacesWon = 0;
            for (int y = 0; y < gridSize; y++) {
                string tile = x + "," + y;
                if (GameObject.Find(tile).CompareTag("Player")) {
                    playerSpacesWon += 1;
                    if (playerSpacesWon == gridSize) {
                        gameState = GameState.PlayerVictory;
                        StartCoroutine(PlayerVictory());
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
                        gameState = GameState.PlayerVictory;
                        StartCoroutine(PlayerVictory());
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
                    gameState = GameState.PlayerVictory;
                    StartCoroutine(PlayerVictory());
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
                    gameState = GameState.PlayerVictory;
                    StartCoroutine(PlayerVictory());
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }


        // Enemy Win Conditions

        // Vertical Win Condition
        for (int x = 0; x < gridSize; x++) {
            enemySpacesWon = 0;
            for (int y = 0; y < gridSize; y++) {
                string tile = x + "," + y;
                if (GameObject.Find(tile).CompareTag("Enemy")) {
                    enemySpacesWon += 1;
                    if (enemySpacesWon == gridSize) {
                        gameState = GameState.EnemyVictory;
                        StartCoroutine(EnemyVictory());
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
            enemySpacesWon = 0;
            for (int x = 0; x < gridSize; x++) {
                string tile = x + "," + y;
                if (GameObject.Find(tile).CompareTag("Enemy")) {
                    enemySpacesWon += 1;
                    if (enemySpacesWon == gridSize) {
                        gameState = GameState.EnemyVictory;
                        StartCoroutine(EnemyVictory());
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
        enemySpacesWon = 0;
        for (int x = 0; x < gridSize; x++) {
            int y = x;

            string tile = x + "," + y;
            if (GameObject.Find(tile).CompareTag("Enemy")) {
                enemySpacesWon += 1;
                if (enemySpacesWon == gridSize) {
                    gameState = GameState.EnemyVictory;
                    StartCoroutine(EnemyVictory());
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }

        // Right-Left Diagonal Win Condition
        enemySpacesWon = 0;
        for (int x = gridSize - 1; x >= 0; x--) {
            int y = (gridSize - 1) - x;

            string tile = x + "," + y;
            if (GameObject.Find(tile).CompareTag("Enemy")) {
                enemySpacesWon += 1;
                if (enemySpacesWon == gridSize) {
                    gameState = GameState.EnemyVictory;
                    StartCoroutine(EnemyVictory());
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }

        if (gameState == GameState.PlayerVictory || gameState == GameState.EnemyVictory) {
            return true;
        }

        return false;
    }
}
