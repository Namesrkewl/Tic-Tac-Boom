using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player, enemy;
    public SetPlayerObject setPlayerObject, setEnemyObject;
    public List<Sprite> exiledSprites, pureSprites, skins;
    public StoryModeAI storyModeAI;
    public GameObject skillMenu, confirmSkillMenu, skills;
    private GameObject playerAtTrigger;

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
            skillMenu = GameObject.Find("SkillMenu");
            confirmSkillMenu = GameObject.Find("ConfirmSkillMenu");
            skills = GameObject.Find("Skills");
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetPlayers(bool vsAI) {
        player = new Player(Player.Type.Player);
        player.faction = Player.Faction.Exiled;
        player.character = Player.Character.Rebel;
        player.talents = new List<Talent>();
        AddTalent(player, Talent.TalentName.SmallBomb);
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
        enemy.talents = new List<Talent>();
        setPlayerObject = GameObject.Find("Player").GetComponent<SetPlayerObject>();
        setEnemyObject = GameObject.Find("Enemy").GetComponent<SetPlayerObject>();
    }

    public void SetCharacterSprite(Player _player) {
        string path;
        if (_player.faction == Player.Faction.Exiled) {
            path = "Characters/Exiled/" + _player.character.ToString();
            _player.characterSprite = Resources.Load<Sprite>(path);
            if (_player.character == Player.Character.Rebel && _player == enemy) {
                _player.playerSprite = Resources.Load<Sprite>(path + "_boss");
            } else {
                _player.playerSprite = _player.characterSprite;
            }
        } else {
            path = "Characters/Pure/" + _player.character.ToString();
            _player.characterSprite = Resources.Load<Sprite>(path);
            if (_player.character == Player.Character.King && _player == enemy) {
                _player.playerSprite = Resources.Load<Sprite>(path + "_boss");
            } else {
                _player.playerSprite = _player.characterSprite;
            }
        }
        if (_player == player) {
            player.playerObject = Resources.Load<PlayerObject>("Prefabs/ScriptableObjects/Players/PlayerObject");
            player.SetPlayerObject();
            setPlayerObject.playerObject = player.playerObject;
        } else if (_player == enemy) {
            enemy.playerObject = Resources.Load<PlayerObject>("Prefabs/ScriptableObjects/Players/EnemyObject");
            enemy.SetPlayerObject();
            setEnemyObject.playerObject = enemy.playerObject;
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
        Debug.Log(player.state);
        Debug.Log(player.remainingMoves);

        if (!playerAtTrigger.activeSelf && !go.tag.Contains("Wall")) {
            if (player.state == Player.State.Playing && player.remainingMoves > 0) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                    player.remainingMoves -= 1;
                } else {
                    ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1]);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    AudioManager.instance.soundEffects.Stop();
                    AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/move_sfx"));
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
                    ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1]);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/move_sfx"));
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
        CancelSkill();
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
            CancelSkill();
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
            CancelSkill();
        }
    }

    void Mine(GameObject go) {
        if (go.tag.Contains("Mine")) {
            TriggerMine(go);
            CancelSkill();
        } else {
            go.tag = "Mine";
            if (player.state == Player.State.Playing) {
                go.GetComponent<SpriteRenderer>().sprite = (from talent in player.talents where talent.talentName == Talent.TalentName.Mine select talent).FirstOrDefault().sprite;
            } else if (enemy.state == Player.State.Playing) {
                go.GetComponent<SpriteRenderer>().sprite = (from talent in enemy.talents where talent.talentName == Talent.TalentName.Mine select talent).FirstOrDefault().sprite;
            }
            go.transform.localScale = Vector3.zero;
            LeanTween.scale(go, Vector3.one, 0.5f).setEaseOutElastic();
            CancelSkill();
        }
    }

    public void BuildTiles() {
        if (StoryManager.instance.newGridSize > 0 && StoryManager.instance.newGridSize < 8) {
            if (player.state == Player.State.Playing) {
                (from talent in player.talents where talent.talentName == Talent.TalentName.BuildTiles select talent).FirstOrDefault();
                StoryManager.instance.newGridSize = StoryManager.instance.gridSize + 1;
                if (StoryManager.instance.newGridSize == 8) StoryManager.instance.newGridSize = 7;
            } else if (enemy.state == Player.State.Playing) {
                StoryManager.instance.newGridSize = StoryManager.instance.gridSize + 1;
                if (StoryManager.instance.newGridSize == 8) StoryManager.instance.newGridSize = 7;
            }
        }
    }
    public void DestroyTiles() {
        if (StoryManager.instance.newGridSize > 0 && StoryManager.instance.newGridSize < 8) {
            if (player.state == Player.State.Playing) {
                StoryManager.instance.newGridSize = StoryManager.instance.gridSize - 1;
                if (StoryManager.instance.newGridSize == 0) StoryManager.instance.newGridSize = 1;
            } else if (enemy.state == Player.State.Playing) {
                StoryManager.instance.newGridSize = StoryManager.instance.gridSize - 1;
                if (StoryManager.instance.newGridSize == 0) StoryManager.instance.newGridSize = 1;
            }
        }
    }

    // Passives

    // Talent Logic
    public void AddTalent(Player _player, Talent.TalentName talentName) {
        _player.talents.Add(new Talent(talentName));
        if (_player == player) {
            _player.talents.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Player/" + talentName.ToString());
            _player.talents.Last().SetTalentObject();
        } else {
            _player.talents.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Enemy/" + talentName.ToString());
            _player.talents.Last().SetTalentObject();
        }
        Debug.Log(_player.talents.Last().talentObject);
    }
    public void SetSkills() {
        for (int i = 0; i < player.talents.Count; i++) {
            if (player.talents[i].type != Talent.Type.Passive) {
                GameObject talent = Instantiate(Resources.Load<GameObject>("Prefabs/Talents/Actives/TALENT"), skills.transform);
                talent.name = player.talents[i].talentName.ToString();
                talent.GetComponent<Image>().sprite = player.talents[i].sprite;
                talent.GetComponent<Button>().onClick.AddListener(delegate{SelectSkill(talent);});
            }
        }
    }
    public void SelectSkill(GameObject skill) {
        confirmSkillMenu.transform.GetChild(1).transform.localScale = new Vector3(0, 0, 0);
        confirmSkillMenu.transform.localPosition = new Vector3(0, 0, 0);
        LeanTween.scale(confirmSkillMenu.transform.GetChild(1).gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic();
        if (player.state == Player.State.Playing) {
            for (int i = 0; i < player.talents.Count; i++) {
                if (skill.name == player.talents[i].talentName.ToString()) {
                    confirmSkillMenu.GetComponent<SetConfirmSkillMenu>().talentObject = player.talents[i].talentObject;
                }
            }
            player.state = Player.State.SelectingSkill;
        } else if (enemy.state != Player.State.Playing) {
            for (int i = 0; i < enemy.talents.Count; i++) {
                if (skill.name == enemy.talents[i].talentName.ToString()) {
                    confirmSkillMenu.GetComponent<SetConfirmSkillMenu>().talentObject = enemy.talents[i].talentObject;
                }
            }
            enemy.state = Player.State.SelectingSkill;
        }
    }
    public void ConfirmSkill() {
        skillMenu.transform.localPosition = new Vector3(0, 3840, 0);
        if (player.state == Player.State.SelectingSkill) {
            player.state = Player.State.UsingSkill;
        } else if (enemy.state == Player.State.SelectingSkill) {
            enemy.state = Player.State.UsingSkill;
        }
    }
    public void CancelSkill() {
        confirmSkillMenu.transform.localPosition = new Vector3(0, -3840, 0);
        skillMenu.transform.localPosition = Vector3.zero;
        if (player.state == Player.State.UsingSkill) {
            player.state = Player.State.Playing;
        } else if (enemy.state == Player.State.UsingSkill) {
            enemy.state = Player.State.Playing;
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
        ParticleSystem boom = Instantiate(ParticleSystemsManager.instance.particleSystems[0]);
        boom.transform.position = go.transform.position;
        boom.Play();
        AudioManager.instance.soundEffects.Stop();
        AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/Explosions/bomb_sfx"));
    }
}
