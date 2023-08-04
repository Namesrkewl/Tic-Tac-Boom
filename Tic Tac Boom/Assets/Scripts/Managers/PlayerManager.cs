using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player, enemy;
    public SetPlayerObject setPlayerObject, setEnemyObject;
    public PlayerObject playerObject, enemyObject;
    public List<Sprite> exiledSprites, pureSprites, skins;
    public StoryModeAI storyModeAI;
    public AudioManager audioManager;
    public ParticleSystemsManager particleSystemsManager;
    private GameObject playerAtTrigger;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void SetPlayers(bool vsAI) {
        player = new Player(Player.Type.Player);
        player.faction = Player.Faction.Exiled;
        player.character = Player.Character.Rebel;
        if (vsAI) {
            enemy = new Player(Player.Type.AI);
        } else {
            enemy = new Player(Player.Type.Player);
        }
        if (player.faction == Player.Faction.Exiled) {
            enemy.faction = Player.Faction.Pure;
        } else {
            enemy.faction = Player.Faction.Exiled;
        }
        setPlayerObject = GameObject.Find("Player").GetComponent<SetPlayerObject>();
        setEnemyObject = GameObject.Find("Enemy").GetComponent<SetPlayerObject>();
    }

    public void SetCharacterSprite(Player _player) {
        if (_player.faction == Player.Faction.Exiled) {
            switch (_player.character) {
                case Player.Character.Basilisk:
                    _player.characterSprite = exiledSprites[0];
                    _player.playerSprite = exiledSprites[0];
                    break;
                case Player.Character.Common:
                    _player.characterSprite = exiledSprites[1];
                    _player.playerSprite = exiledSprites[1];
                    break;
                case Player.Character.Dragon:
                    _player.characterSprite = exiledSprites[2];
                    _player.playerSprite = exiledSprites[2];
                    break;
                case Player.Character.Engineer:
                    _player.characterSprite = exiledSprites[3];
                    _player.playerSprite = exiledSprites[3];
                    break;
                case Player.Character.Fragmented:
                    _player.characterSprite = exiledSprites[4];
                    _player.playerSprite = exiledSprites[4];
                    break;
                case Player.Character.King:
                    _player.characterSprite = exiledSprites[5];
                    _player.playerSprite = exiledSprites[5];
                    break;
                case Player.Character.Knight:
                    _player.characterSprite = exiledSprites[6];
                    _player.playerSprite = exiledSprites[6];
                    break;
                case Player.Character.Nimble:
                    _player.characterSprite = exiledSprites[7];
                    _player.playerSprite = exiledSprites[7];
                    break;
                case Player.Character.Peasant:
                    _player.characterSprite = exiledSprites[8];
                    _player.playerSprite = exiledSprites[8];
                    break;
                case Player.Character.Rebel:
                    _player.characterSprite = exiledSprites[9];
                    _player.playerSprite = exiledSprites[9];
                    break;
                case Player.Character.Thief:
                    _player.characterSprite = exiledSprites[10];
                    _player.playerSprite = exiledSprites[10];
                    break;
                case Player.Character.Vagabond:
                    _player.characterSprite = exiledSprites[11];
                    _player.playerSprite = exiledSprites[11];
                    break;
                case Player.Character.Wizard:
                    _player.characterSprite = exiledSprites[12];
                    _player.playerSprite = exiledSprites[12];
                    break;
                case Player.Character.Wraith:
                    _player.characterSprite = exiledSprites[13];
                    _player.playerSprite = exiledSprites[13];
                    break;
            }
        } else {
            switch (_player.character) {
                case Player.Character.Basilisk:
                    _player.characterSprite = pureSprites[0];
                    _player.playerSprite = pureSprites[0];
                    break;
                case Player.Character.Common:
                    _player.characterSprite = pureSprites[1];
                    _player.playerSprite = pureSprites[1];
                    break;
                case Player.Character.Dragon:
                    _player.characterSprite = pureSprites[2];
                    _player.playerSprite = pureSprites[2];
                    break;
                case Player.Character.Engineer:
                    _player.characterSprite = pureSprites[3];
                    _player.playerSprite = pureSprites[3];
                    break;
                case Player.Character.Fragmented:
                    _player.characterSprite = pureSprites[4];
                    _player.playerSprite = pureSprites[4];
                    break;
                case Player.Character.King:
                    _player.characterSprite = pureSprites[5];
                    _player.playerSprite = pureSprites[5];
                    break;
                case Player.Character.Knight:
                    _player.characterSprite = pureSprites[6];
                    _player.playerSprite = pureSprites[6];
                    break;
                case Player.Character.Nimble:
                    _player.characterSprite = pureSprites[7];
                    _player.playerSprite = pureSprites[7];
                    break;
                case Player.Character.Peasant:
                    _player.characterSprite = pureSprites[8];
                    _player.playerSprite = pureSprites[8];
                    break;
                case Player.Character.Rebel:
                    _player.characterSprite = pureSprites[9];
                    _player.playerSprite = pureSprites[9];
                    break;
                case Player.Character.Thief:
                    _player.characterSprite = pureSprites[10];
                    _player.playerSprite = pureSprites[10];
                    break;
                case Player.Character.Vagabond:
                    _player.characterSprite = pureSprites[11];
                    _player.playerSprite = pureSprites[11];
                    break;
                case Player.Character.Wizard:
                    _player.characterSprite = pureSprites[12];
                    _player.playerSprite = pureSprites[12];
                    break;
                case Player.Character.Wraith:
                    _player.characterSprite = pureSprites[13];
                    _player.playerSprite = pureSprites[13];
                    break;
            }
        }
        if (_player == player) {
            player.playerObject = playerObject;
            player.SetPlayerObject();
            setPlayerObject.playerObject = player.playerObject;
            Debug.Log("Player Set");
        } else if (_player == enemy) {
            enemy.playerObject = enemyObject;
            enemy.SetPlayerObject();
            setEnemyObject.playerObject = enemy.playerObject;
            Debug.Log("Enemy Set");
        }

    }
    public void SetAI(Player _player) {
        switch (_player.character) {
            case Player.Character.Basilisk:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 5;
                _player.maxMoves = 2;
                break;
            case Player.Character.Common:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 3;
                _player.maxMoves = 1;
                break;
            case Player.Character.Dragon:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 7;
                _player.maxMoves = 6;
                break;
            case Player.Character.Engineer:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 4;
                _player.maxMoves = 1;
                break;
            case Player.Character.Fragmented:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 5;
                _player.maxMoves = 2;
                break;
            case Player.Character.King:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 7;
                _player.maxMoves = 3;
                break;
            case Player.Character.Knight:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = 30;
                storyModeAI.baseValue = 0;
                storyModeAI.playerValue = 50;
                storyModeAI.impendingVictoryValue = 1000;
                storyModeAI.impendingDoomValue = 250;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 5;
                _player.maxMoves = 3;
                break;
            case Player.Character.Nimble:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 5;
                _player.maxMoves = 3;
                break;
            case Player.Character.Peasant:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 3;
                _player.maxMoves = 1;
                break;
            case Player.Character.Rebel:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 7;
                _player.maxMoves = 3;
                break;
            case Player.Character.Thief:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 3;
                _player.maxMoves = 2;
                break;
            case Player.Character.Vagabond:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 5;
                _player.maxMoves = 3;
                break;
            case Player.Character.Wizard:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 4;
                _player.maxMoves = 2;
                break;
            case Player.Character.Wraith:
                storyModeAI.blockedValue = -50;
                storyModeAI.enemyValue = -5;
                storyModeAI.baseValue = 1;
                storyModeAI.playerValue = 20;
                storyModeAI.impendingVictoryValue = 999;
                storyModeAI.impendingDoomValue = 9999;
                storyModeAI.victoryValue = 99999;
                storyModeAI.startingGridSize = 4;
                _player.maxMoves = 2;
                break;
            default:
                break;
        }
    }
    public void SetSkins() {
        if (player.faction == Player.Faction.Exiled) {
            player.skin = skins[0];
            enemy.skin = skins[1];
        } else {
            player.skin = skins[1];
            enemy.skin = skins[0];
        }
    }

    // Add the PlayerMove event to the EventManager at the on scene load
    private void Start() {
        EventManager.PlayerMoveEvent += StartPlayerMove;
    }

    public void StartPlayerMove(GameObject go) {
        StartCoroutine(PlayerMove(go));
    }

    // Player Movement logic at the selected tile (go)
    IEnumerator PlayerMove(GameObject go) {
        if (go == null) {
            yield return null;
        }
        playerAtTrigger = go.transform.GetChild(0).gameObject;

        if (!playerAtTrigger.activeSelf && !go.tag.Contains("Wall")) {
            if (player.state == Player.State.Playing && player.remainingMoves > 0) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                    player.remainingMoves -= 1;
                } else {
                    ParticleSystem dust = Instantiate(particleSystemsManager.particleSystems[1]);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    audioManager.soundEffects.Stop();
                    audioManager.soundEffects.PlayOneShot(audioManager.moveSounds[0]);
                    go.tag = "Player";
                    player.remainingMoves -= 1;
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = player.skin;
                    playerAtTrigger.SetActive(true);
                }
            } else if (enemy.state == Player.State.Playing && enemy.remainingMoves > 0) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                    enemy.remainingMoves -= 1;
                } else {
                    ParticleSystem dust = Instantiate(particleSystemsManager.particleSystems[1]);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    audioManager.soundEffects.PlayOneShot(audioManager.moveSounds[0]);
                    go.tag = "Enemy";
                    enemy.remainingMoves -= 1;
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = enemy.skin;
                    playerAtTrigger.SetActive(true);
                }
            }
        } else if (player.state == Player.State.UsingSkill) {
            switch (Player.activeTalent.talentName) {
                case Talent.TalentName.SmallBomb:
                    SmallBomb(go);
                    break;
                case Talent.TalentName.CrossBomb:
                    CrossBomb(go);
                    break;
                case Talent.TalentName.XBomb:
                    XBomb(go);
                    break;
                case Talent.TalentName.Mine:
                    Mine(go);
                    break;
            }
        }
        yield return null;
    }

    // TALENTS

    // ACTIVES ABILITIES

    void SmallBomb(GameObject go) {
        if (go.tag.Contains("Mine")) {
            TriggerMine(go);
        }
        BombTrigger(go);
        playerAtTrigger.GetComponent<SpriteRenderer>().sprite = null;
        playerAtTrigger.SetActive(false);
        go.tag = "Untagged";
        //GameManager.instance.talents.CancelSkill();
    }

    void CrossBomb(GameObject go) {

        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        if (GameObject.Find(x + "," + y) != null) {
            // Bombs the selected tile
            if (go.tag.Contains("Mine")) {
                TriggerMine(go);
            }
            BombTrigger(go);
            go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.tag = "Untagged";

            // Bombs surrounding tiles in a + shape
            if (GameObject.Find((x + 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            //Talent.CancelSkill();
        }

    }

    void XBomb(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        if (GameObject.Find(x + "," + y) != null) {
            // Bombs the selected tile
            if (go.tag.Contains("Mine")) {
                TriggerMine(go);
            }
            BombTrigger(go);
            go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.tag = "Untagged";

            // Bombs surrounding tiles in a x shape
            if (GameObject.Find((x + 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x + 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            //GameManager.instance.talents.CancelSkill();
        }
    }

    void Mine(GameObject go) {
        if (go.tag.Contains("Mine")) {
            TriggerMine(go);
            //GameManager.instance.talents.CancelSkill();
        } else {
            go.tag = "Mine";
            go.GetComponent<SpriteRenderer>().sprite = GameManager.instance.mineSprite;
            go.transform.localScale = Vector3.zero;
            LeanTween.scale(go, Vector3.one, 0.5f).setEaseOutElastic();
            //GameManager.instance.talents.CancelSkill();
        }
    }

    void TriggerMine(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        if (GameObject.Find(x + "," + y) != null) {
            // Bombs the selected tile
            BombTrigger(go);
            go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.tag = "Untagged";
            go.GetComponent<SpriteRenderer>().sprite = null;

            // Bombs surrounding tiles in a x shape
            if (GameObject.Find((x + 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x + 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            // Bombs surrounding tiles in a + shape
            if (GameObject.Find((x + 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
        }
    }

    void BombTrigger(GameObject go) {
        ParticleSystem boom = Instantiate(particleSystemsManager.particleSystems[0]);
        boom.transform.position = go.transform.position;
        boom.Play();
        audioManager.soundEffects.Stop();
        audioManager.soundEffects.PlayOneShot(audioManager.bombSounds[0]);
    }
}
