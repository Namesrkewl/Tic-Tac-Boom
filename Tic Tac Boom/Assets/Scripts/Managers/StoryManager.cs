using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    
    public GameObject UI, HUD, nextFight, grid, turnDisplay, playerVictoryMenu, enemyVictoryMenu, levelClearMenu;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        state = State.Loading;
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
        Idle,
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

        yield return StartCoroutine(EndGame());

        yield return null;
    }

    private IEnumerator EndGame() {
        switch (state) {
            case State.Draw:
                yield return StartCoroutine(Draw());
                break;
            case State.PlayerVictory:
                yield return StartCoroutine(PlayerVictory());
                break;
            case State.EnemyVictory:
                yield return StartCoroutine(EnemyVictory());
                break;
        }

        yield return null;
    }

    public IEnumerator NewGame() {
        PlayerManager.instance.SetPlayers(Player.Faction.Exiled, Player.Character.Rebel, Player.Type.AI);
        PlayerManager.instance.SetSkins();
        PlayerManager.instance.player.maxMoves = 1;
        GameManager.instance.ResetTalents();
        PlayerManager.instance.player.SetTalentPools();
        GameManager.instance.turn = 1;
        GameManager.instance.round = 1;
        GameManager.instance.stage = 1;
        GameManager.instance.gridSize = 7;
        GameManager.instance.newGridSize = 7;
        GameManager.instance.minGridSize = 1;
        GameManager.instance.maxGridSize = 7;
        state = State.Loading;
        yield return StartCoroutine(Loading());
        yield return null;
    }

    private IEnumerator Loading() {
        PlayerManager.instance.SetCharacterSprite(PlayerManager.instance.player);
        PlayerManager.instance.enemy.character = Player.Character.Peasant;
        PlayerManager.instance.SetCharacterSprite(PlayerManager.instance.enemy);
        PlayerManager.instance.SetAI(PlayerManager.instance.enemy);
        PlayerManager.instance.AddTalent(PlayerManager.instance.player, Talent.TalentName.SmallBomb);
        PlayerManager.instance.SetTalents();
        GameManager.instance.newGridSize = PlayerManager.instance.storyModeAI.startingGridSize;
        GameManager.instance.gridSize = GameManager.instance.newGridSize;
        GridManager.instance.state = GridManager.State.Generating;
        StartCoroutine(GridManager.instance.GenerateGrid(GameManager.instance.gridSize));
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
        yield return StartCoroutine(LevelStart());
        yield return null;
    }

    private IEnumerator LevelStart() {
        if (PlayerManager.instance.player.faction == Player.Faction.Exiled) {
            state = State.PlayerTurn;
        } else {
            state = State.EnemyTurn;
        }
        yield return StartCoroutine(StoryMode());
        yield return null;
    }

    private IEnumerator PlayerTurn() {
        if (PlayerManager.instance.player.state == Player.State.Inactive) {
            PlayerManager.instance.player.state = Player.State.Playing;
            PlayerManager.instance.player.Cooldown();
            PlayerManager.instance.player.remainingMoves = PlayerManager.instance.player.maxMoves;
            turnDisplay.transform.GetChild(0).gameObject.SetActive(true);
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
            PlayerManager.instance.enemy.Cooldown();
            PlayerManager.instance.enemy.remainingMoves = PlayerManager.instance.enemy.maxMoves;
            turnDisplay.transform.GetChild(1).gameObject.SetActive(true);
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
        GameManager.instance.turn++;
        GameManager.instance.round = GameManager.instance.turn / 2;
        if (PlayerManager.instance.player.state != Player.State.Inactive) {
            PlayerManager.instance.player.state = Player.State.Inactive;
            turnDisplay.transform.GetChild(0).gameObject.SetActive(false);
            state = State.Idle;
            yield return new WaitForSeconds(1f);
            state = State.EnemyTurn;
        } else if (PlayerManager.instance.enemy.state != Player.State.Inactive) {
            PlayerManager.instance.enemy.state = Player.State.Inactive;
            turnDisplay.transform.GetChild(1).gameObject.SetActive(false);
            state = State.Idle;
            yield return new WaitForSeconds(1f);
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
        levelClearMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"LEVEL {GameManager.instance.stage}";
        yield return null;
    }

    private IEnumerator NextFight() {
        GameManager.instance.stage += 1;
        yield return null;
    }

    private IEnumerator GridLocked() {
        Debug.Log("GridLocked! The game is over for now, until logic is inserted for how this will be handled.");
        yield return null;
    }

    private IEnumerator PlayerVictory() {
        StopPlayers();
        if (GameManager.instance.stage < 15) {
            yield return StartCoroutine(StageClear());
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
        yield return StartCoroutine(GridManager.instance.ResolveDraw());
        yield return new WaitForSeconds(2f);
        if (PlayerManager.instance.player.state != Player.State.Inactive) {
            state = State.EnemyTurn;
        } else if (PlayerManager.instance.enemy.state != Player.State.Inactive) {
            state = State.PlayerTurn;
        }
        yield return StartCoroutine(StoryMode());
        yield return null;
    }
    public bool GameOver() {
        if (SceneManager.GetActiveScene().name != "StoryMode") {
            return true;
        }

        int playerSpacesWon;
        int enemySpacesWon;
        bool gridLocked;

        if (GridManager.instance.state != GridManager.State.Idle) {
            return false;
        }
        // Player Win Conditions

        // Vertical Win Condition
        for (int x = 0; x < GameManager.instance.gridSize; x++) {
            playerSpacesWon = 0;
            for (int y = 0; y < GameManager.instance.gridSize; y++) {
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                    playerSpacesWon += 1;
                    if (playerSpacesWon == GameManager.instance.gridSize) {
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
        for (int y = 0; y < GameManager.instance.gridSize; y++) {
            playerSpacesWon = 0;
            for (int x = 0; x < GameManager.instance.gridSize; x++) {
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                    playerSpacesWon += 1;
                    if (playerSpacesWon == GameManager.instance.gridSize) {
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
        for (int x = 0; x < GameManager.instance.gridSize; x++) {
            int y = x;
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                playerSpacesWon += 1;
                if (playerSpacesWon == GameManager.instance.gridSize) {
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
        for (int x = GameManager.instance.gridSize - 1; x >= 0; x--) {
            int y = (GameManager.instance.gridSize - 1) - x;
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Player")) {
                playerSpacesWon += 1;
                if (playerSpacesWon == GameManager.instance.gridSize) {
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
        for (int x = 0; x < GameManager.instance.gridSize; x++) {
            enemySpacesWon = 0;
            for (int y = 0; y < GameManager.instance.gridSize; y++) {
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                    enemySpacesWon += 1;
                    if (enemySpacesWon == GameManager.instance.gridSize) {
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
        for (int y = 0; y < GameManager.instance.gridSize; y++) {
            enemySpacesWon = 0;
            for (int x = 0; x < GameManager.instance.gridSize; x++) {
                if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                    enemySpacesWon += 1;
                    if (enemySpacesWon == GameManager.instance.gridSize) {
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
        for (int x = 0; x < GameManager.instance.gridSize; x++) {
            int y = x;
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                enemySpacesWon += 1;
                if (enemySpacesWon == GameManager.instance.gridSize) {
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
        for (int x = GameManager.instance.gridSize - 1; x >= 0; x--) {
            int y = (GameManager.instance.gridSize - 1) - x;
            if (GridManager.instance.Tiles[x][y].transform.CompareTag("Enemy")) {
                enemySpacesWon += 1;
                if (enemySpacesWon == GameManager.instance.gridSize) {
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
            turnDisplay.transform.GetChild(0).gameObject.SetActive(false);
            turnDisplay.transform.GetChild(1).gameObject.SetActive(false);
            return true;
        } else if (state == State.PlayerVictory || state == State.EnemyVictory) {
            turnDisplay.transform.GetChild(0).gameObject.SetActive(false);
            turnDisplay.transform.GetChild(1).gameObject.SetActive(false);
            return true;
        } else {
            gridLocked = true;
            for (int x = 0; x < GameManager.instance.gridSize; x++) {
                for (int y = 0; y < GameManager.instance.gridSize; y++) {
                    if (GridManager.instance.Tiles[x][y].transform.CompareTag("Untagged") || GridManager.instance.Tiles[x][y].transform.CompareTag("Mine")) {
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
