using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    public int turn, round, stage, gridSize, newGridSize;
    public bool updatingGrid;
    public GameObject UI, HUD, nextFight, grid, turnDisplay, playerVictoryMenu, enemyVictoryMenu, levelClearMenu;

    private void Awake() {
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
        if (SceneManager.GetActiveScene().name == "StoryMode") {
            UI = GameObject.Find("UI");
            HUD = GameObject.Find("HUD");
            nextFight = GameObject.Find("NextFight");
            grid = GameObject.Find("Grid");
            turnDisplay = GameObject.Find("TurnDisplay");
            playerVictoryMenu = GameObject.Find("PlayerVictoryMenu");
            enemyVictoryMenu = GameObject.Find("EnemyVictoryMenu");
            levelClearMenu = GameObject.Find("LevelClearMenu");
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update() {
        //Debug.Log(Player.activeTalent);
    }

    public State state;
    public enum State {
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
            switch (state) {
                case State.EndTurn:
                    yield return StartCoroutine(EndTurn());
                    break;
                case State.EnemyTurn:
                    yield return StartCoroutine(EnemyTurn());
                    break;
                case State.GridLocked:
                    yield return StartCoroutine(GridLocked());
                    break;
                case State.PlayerTurn:
                    yield return StartCoroutine(PlayerTurn());
                    break;
                default:
                    break;
            }
            yield return null;
        }

        StartCoroutine(EndGame());

        yield return null;
    }

    private IEnumerator EndGame() {
        switch (state) {
            case State.Draw:
                StartCoroutine(Draw());
                break;
            case State.PlayerVictory:
                StartCoroutine(PlayerVictory());
                break;
            case State.EnemyVictory:
                StartCoroutine(EnemyVictory());
                break;
        }

        yield return null;
    }

    public void NewGame() {
        PlayerManager.instance.SetPlayers(true);
        PlayerManager.instance.SetSkins();
        PlayerManager.instance.player.maxMoves = 1;
        turn = 1;
        round = 1;
        stage = 1;
        gridSize = 3;
        newGridSize = 3;
        state = State.Loading;
        StartCoroutine(Loading());
    }

    private IEnumerator Loading() {
        PlayerManager.instance.SetCharacterSprite(PlayerManager.instance.player);
        PlayerManager.instance.enemy.character = Player.Character.Thief;
        PlayerManager.instance.SetCharacterSprite(PlayerManager.instance.enemy);
        PlayerManager.instance.SetAI(PlayerManager.instance.enemy);
        PlayerManager.instance.SetSkills();
        newGridSize = PlayerManager.instance.storyModeAI.startingGridSize;
        gridSize = newGridSize;
        GridManager.instance.state = GridManager.State.Generating;
        StartCoroutine(GridManager.instance.GenerateGrid(gridSize));
        UI.GetComponent<CanvasGroup>().alpha = 0;
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
        AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/wind_sfx"));
        yield return new WaitForSeconds(0.5f);
        LeanTween.alpha(background, 0, 1f);
        yield return new WaitForSeconds(1f);
        background.SetActive(false);
        AudioManager.instance.soundEffects.Stop();
        if (AudioManager.instance.backgroundMusic.clip != Resources.Load<AudioClip>("Sounds/Music/loading_theme")) {
            AudioManager.instance.backgroundMusic.clip = Resources.Load<AudioClip>("Sounds/Music/loading_theme");
            AudioManager.instance.backgroundMusic.Play();
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
        AudioManager.instance.backgroundMusic.Stop();
        AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/wind_sfx"));
        LeanTween.alpha(darkVeil, 0, 1f);
        yield return new WaitForSeconds(2f);
        darkVeil.SetActive(false);
        AudioManager.instance.backgroundMusic.Stop();
        AudioManager.instance.backgroundMusic.clip = Resources.Load<AudioClip>("Sounds/Music/battle_theme");
        AudioManager.instance.backgroundMusic.Play();
        cover.SetActive(false);
        playerIndicator.SetActive(true);
        enemyIndicator.SetActive(true);
        turnIndicator.SetActive(true);
        UI.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(1f);
        state = State.LevelStart;
        StartCoroutine(LevelStart());
        yield return null;
    }

    private IEnumerator LevelStart() {
        if (PlayerManager.instance.player.faction == Player.Faction.Exiled) {
            state = State.PlayerTurn;
        } else {
            state = State.EnemyTurn;
        }
        StartCoroutine(StoryMode());
        yield return null;
    }

    private IEnumerator PlayerTurn() {
        if (PlayerManager.instance.player.state == Player.State.Inactive) {
            PlayerManager.instance.player.state = Player.State.Playing;
            PlayerManager.instance.enemy.state = Player.State.Inactive;
            PlayerManager.instance.player.remainingMoves = PlayerManager.instance.player.maxMoves;
            turnDisplay.transform.GetChild(0).gameObject.SetActive(true);
            turnDisplay.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        if (PlayerManager.instance.player.remainingMoves <= 0) {
            PlayerManager.instance.player.state = Player.State.Idle;
            state = State.EndTurn;
        }

        yield return null;
    }

    private IEnumerator EnemyTurn() {
        if (PlayerManager.instance.enemy.state == Player.State.Inactive) {
            PlayerManager.instance.enemy.state = Player.State.Playing;
            PlayerManager.instance.player.state = Player.State.Inactive;
            PlayerManager.instance.enemy.remainingMoves = PlayerManager.instance.enemy.maxMoves;
            turnDisplay.transform.GetChild(1).gameObject.SetActive(true);
            turnDisplay.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (PlayerManager.instance.enemy.remainingMoves > 0) {
            PlayerManager.instance.StartPlayerMove(PlayerManager.instance.storyModeAI.AIMove());
            yield return new WaitForSeconds(0.2f);
        } else {
            PlayerManager.instance.enemy.state = Player.State.Idle;
            state = State.EndTurn;
        }

        yield return null;
    }

    private IEnumerator EndTurn() {
        turn++;
        round = turn / 2;
        if (PlayerManager.instance.player.state != Player.State.Inactive) {
            state = State.EnemyTurn;
        } else if (PlayerManager.instance.enemy.state != Player.State.Inactive) {
            state = State.PlayerTurn;
        }
        yield return null;
    }

    private IEnumerator StageClear() {
        yield return new WaitForSeconds(2f);
        UI.GetComponent<CanvasGroup>().alpha = 0;
        GameObject turnIndicator = HUD.transform.GetChild(0).gameObject;
        turnIndicator.SetActive(false);
        GameObject playerIndicator = HUD.transform.GetChild(1).GetChild(0).gameObject;
        playerIndicator.SetActive(false);
        GameObject enemyIndicator = HUD.transform.GetChild(2).GetChild(0).gameObject;
        enemyIndicator.SetActive(false);
        GameObject rollingFog = nextFight.transform.GetChild(3).gameObject;
        rollingFog.GetComponent<ParticleSystem>().Play();
        AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/wind_sfx"));
        yield return new WaitForSeconds(1f);
        /*
        talentChoices.ClearTalentChoices();
        talentChoices.GenerateSkills(3);*/
        levelClearMenu.transform.localPosition = Vector3.zero;
        levelClearMenu.transform.GetChild(0).gameObject.SetActive(true);
        yield return null;
    }

    private IEnumerator NextFight() {
        yield return null;
    }

    private IEnumerator GridLocked() {
        Debug.Log("GridLocked! The game is over for now, until logic is inserted for how this will be handled.");
        yield return null;
    }

    private IEnumerator PlayerVictory() {
        StopPlayers();
        if (stage < 15) {
            StartCoroutine(StageClear());
        } else {
            playerVictoryMenu.transform.localPosition = Vector3.zero;
        }
        yield return null;
    }

    private IEnumerator EnemyVictory() {
        StopPlayers();
        enemyVictoryMenu.transform.localPosition = Vector3.zero;
        yield return null;
    }

    private IEnumerator Draw() {
        Debug.Log("Draw! Wiping the board, turn is passed to the next player");
        StartCoroutine(GridManager.instance.ResolveDraw());
        yield return new WaitForSeconds(2f);
        if (PlayerManager.instance.player.state != Player.State.Inactive) {
            state = State.EnemyTurn;
        } else if (PlayerManager.instance.enemy.state != Player.State.Inactive) {
            state = State.PlayerTurn;
        }
        StartCoroutine(StoryMode());
        yield return null;
    }
    public bool GameOver() {
        if (SceneManager.GetActiveScene().name != "StoryMode") {
            return true;
        }

        int playerSpacesWon;
        int enemySpacesWon;
        bool gridLocked;

        if (updatingGrid) {
            return false;
        }
        // Player Win Conditions

        // Vertical Win Condition
        for (int x = 0; x < gridSize; x++) {
            playerSpacesWon = 0;
            for (int y = 0; y < gridSize; y++) {
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                    playerSpacesWon += 1;
                    if (playerSpacesWon == gridSize) {
                        state = State.PlayerVictory;
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
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                    playerSpacesWon += 1;
                    if (playerSpacesWon == gridSize) {
                        state = State.PlayerVictory;
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
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                playerSpacesWon += 1;
                if (playerSpacesWon == gridSize) {
                    state = State.PlayerVictory;
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
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                playerSpacesWon += 1;
                if (playerSpacesWon == gridSize) {
                    state = State.PlayerVictory;
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
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                    enemySpacesWon += 1;
                    if (enemySpacesWon == gridSize) {
                        if (state == State.PlayerVictory) {
                            state = State.Draw;
                        } else {
                            state = State.EnemyVictory;
                        }
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
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                    enemySpacesWon += 1;
                    if (enemySpacesWon == gridSize) {
                        if (state == State.PlayerVictory) {
                            state = State.Draw;
                        } else {
                            state = State.EnemyVictory;
                        }
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
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                enemySpacesWon += 1;
                if (enemySpacesWon == gridSize) {
                    if (state == State.PlayerVictory) {
                        state = State.Draw;
                    } else {
                        state = State.EnemyVictory;
                    }
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
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                enemySpacesWon += 1;
                if (enemySpacesWon == gridSize) {
                    if (state == State.PlayerVictory) {
                        state = State.Draw;
                    } else {
                        state = State.EnemyVictory;
                    }
                    break;
                } else {
                    continue;
                }
            } else {
                break;
            }
        }

        if (state == State.Draw) {
            return true;
        } else if (state == State.PlayerVictory || state == State.EnemyVictory) {
            return true;
        } else {
            gridLocked = true;
            for (int x = 0; x < gridSize; x++) {
                for (int y = 0; y < gridSize; y++) {
                    if (GridManager.instance.Tiles[x][y].transform.CompareTag("Untagged")) {
                        gridLocked = false;
                    }
                }
            }
            if (gridLocked) {
                state = State.GridLocked;
            }
        }

        return false;
    }
    public void StopPlayers() {
        PlayerManager.instance.player.state = Player.State.Inactive;
        PlayerManager.instance.enemy.state = Player.State.Inactive;
    }
}
