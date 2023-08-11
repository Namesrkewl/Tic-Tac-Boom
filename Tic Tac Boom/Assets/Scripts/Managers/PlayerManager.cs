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
    public GameObject skillMenu, confirmSkillMenu, selectSkillMenu, skills;
    private GameObject playerAtTrigger;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        player = new Player();
        enemy = new Player();
    }

    // Add the PlayerMove event to the EventManager at the on scene load
    private void Start() {
        EventManager.PlayerMoveEvent += StartPlayerMove;
    }

    public void StartPlayerMove(GameObject go) {
        StartCoroutine(PlayerMove(go));
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name == "StoryMode") {
            skillMenu = GameObject.Find("SkillMenu");
            confirmSkillMenu = GameObject.Find("ConfirmSkillMenu");
            selectSkillMenu = GameObject.Find("SelectSkillMenu");
            skills = GameObject.Find("Skills");
            setPlayerObject = GameObject.Find("Player").GetComponent<SetPlayerObject>();
            setEnemyObject = GameObject.Find("Enemy").GetComponent<SetPlayerObject>();
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetPlayers(Player.Faction faction, Player.Character character, Player.Type type) {
        player.type = Player.Type.Player;
        player.faction = faction;
        player.character = character;
        player.initialCooldown = 3;
        if (type == Player.Type.AI) {
            enemy.type = Player.Type.AI;
        } else {
            enemy.type = Player.Type.Player;
        }
        if (faction == Player.Faction.Exiled) {
            enemy.faction = Player.Faction.Pure;
            enemy.initialCooldown = 2;
        } else {
            enemy.faction = Player.Faction.Exiled;
            enemy.initialCooldown = 3;
        }
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

    // Player Movement logic at the selected tile (go)
    IEnumerator PlayerMove(GameObject go) {
        if (go == null) {
            yield return null;
        }
        playerAtTrigger = go.transform.GetChild(0).gameObject;
        Debug.Log(player.state);
        Debug.Log(player.remainingMoves);

        if (go.tag.Contains("Mine")) {
            if (player.state == Player.State.Playing && player.remainingMoves > 0) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                    player.remainingMoves -= 1;
                }
            } else if (enemy.state == Player.State.Playing && enemy.remainingMoves > 0) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                    enemy.remainingMoves -= 1;
                }
            }
        } else if (!playerAtTrigger.activeSelf && !go.tag.Contains("Wall") && (player.state == Player.State.Playing || enemy.state == Player.State.Playing)) {
            if (player.state == Player.State.Playing && player.remainingMoves > 0) {
                ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1]);
                dust.transform.position = go.transform.position;
                dust.Play();
                AudioManager.instance.soundEffects.Stop();
                AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/move_sfx"));
                go.tag = "Player";
                player.remainingMoves -= 1;
                playerAtTrigger.GetComponent<SpriteRenderer>().sprite = player.skin;
                playerAtTrigger.SetActive(true);
            } else if (enemy.state == Player.State.Playing && enemy.remainingMoves > 0) {
                ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1]);
                dust.transform.position = go.transform.position;
                dust.Play();
                AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/move_sfx"));
                go.tag = "Enemy";
                enemy.remainingMoves -= 1;
                playerAtTrigger.GetComponent<SpriteRenderer>().sprite = enemy.skin;
                playerAtTrigger.SetActive(true);
            } 
        } else if (player.state == Player.State.UsingSkill || enemy.state == Player.State.UsingSkill) {
            Skill(go);
        }
        yield return null;
    }

    // TALENTS

    // ACTIVES ABILITIES

    private void Skill(GameObject go) {
        if (player.state == Player.State.UsingSkill) {
            switch (player.activeSkill.talentName) {
                case Talent.TalentName.BuildTiles:
                    BuildTiles(go);
                    break;
                case Talent.TalentName.CrossBomb:
                    CrossBomb(go);
                    break;
                case Talent.TalentName.DestroyTiles:
                    DestroyTiles(go);
                    break;
                case Talent.TalentName.Mine:
                    Mine(go);
                    break;
                case Talent.TalentName.SmallBomb:
                    SmallBomb(go);
                    break;
                case Talent.TalentName.XBomb:
                    XBomb(go);
                    break;
            }
        } else if (enemy.state == Player.State.UsingSkill) {
            switch (enemy.activeSkill.talentName) {
                case Talent.TalentName.BuildTiles:
                    BuildTiles(go);
                    break;
                case Talent.TalentName.CrossBomb:
                    CrossBomb(go);
                    break;
                case Talent.TalentName.DestroyTiles:
                    DestroyTiles(go);
                    break;
                case Talent.TalentName.Mine:
                    Mine(go);
                    break;
                case Talent.TalentName.SmallBomb:
                    SmallBomb(go);
                    break;

                case Talent.TalentName.XBomb:
                    XBomb(go);
                    break;
            }
        }
    }

    void SmallBomb(GameObject go) {
        if (go.tag.Contains("Mine")) {
            TriggerMine(go);
        }
        BombTrigger(go);
        playerAtTrigger.GetComponent<SpriteRenderer>().sprite = null;
        playerAtTrigger.SetActive(false);
        go.tag = "Untagged";
        UsedSkill();
    }

    void CrossBomb(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);
        GameObject tile = GridManager.instance.Tiles[x][y];
        if (tile.tag.Contains("Mine")) {
            TriggerMine(go);
        }
        BombTrigger(tile);
        tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        tile.transform.GetChild(0).gameObject.SetActive(false);
        tile.tag = "Untagged";

        for (int i = (x - 1); i < (x + 2); i++) {
            if (i != x && i >= 0 && i < GameManager.instance.gridSize) {
                tile = GridManager.instance.Tiles[i][y];
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(go);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
        }
        for (int n = (y - 1); n < (y + 2); n++) {
            if (n != y && n >= 0 && n < GameManager.instance.gridSize) {
                tile = GridManager.instance.Tiles[x][n];
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(go);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
        }
        UsedSkill();
    }

    void XBomb(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);
        GameObject tile = GridManager.instance.Tiles[x][y];
        if (tile.tag.Contains("Mine")) {
            TriggerMine(go);
        }
        BombTrigger(tile);
        tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        tile.transform.GetChild(0).gameObject.SetActive(false);
        tile.tag = "Untagged";

        for (int i = (x - 1); i < (x + 2); i++) {
            for (int n = (y - 1); n < (y + 2); n++) {
                if (i != x && n != y && (i >= 0 && n >= 0) && (i < GameManager.instance.gridSize && n < GameManager.instance.gridSize)) {
                    tile = GridManager.instance.Tiles[i][n];
                    if (tile.tag.Contains("Mine")) {
                        TriggerMine(go);
                    }
                    BombTrigger(GridManager.instance.Tiles[i][n]);
                    tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                    tile.transform.GetChild(0).gameObject.SetActive(false);
                    tile.tag = "Untagged";
                }
            }
        }

        for (int i = (x - 1); i < (x + 2); i++) {
            for (int n = (y + 1); n > (y - 2); n--) {
                if (i != x && n != y && (i >= 0 && n >= 0) && (i < GameManager.instance.gridSize && n < GameManager.instance.gridSize)) {
                    tile = GridManager.instance.Tiles[i][n];
                    if (tile.tag.Contains("Mine")) {
                        TriggerMine(go);
                    }
                    BombTrigger(GridManager.instance.Tiles[i][n]);
                    tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                    tile.transform.GetChild(0).gameObject.SetActive(false);
                    tile.tag = "Untagged";
                }
            }
        }
        UsedSkill();
    }

    void Mine(GameObject go) {
        if (go.tag.Contains("Mine")) {
            TriggerMine(go);
            UsedSkill();
        } else {
            if (!playerAtTrigger.activeSelf) {
                go.tag = "Mine";
                if (player.state == Player.State.UsingSkill) {
                    for (int i = 0; i < player.skills.Count; i++) {
                        if (player.skills[i].talentName == Talent.TalentName.Mine) {
                            playerAtTrigger.GetComponent<SpriteRenderer>().sprite = player.skills[i].sprite;
                        }
                    }
                } else if (enemy.state == Player.State.UsingSkill) {
                    for (int i = 0; i < enemy.skills.Count; i++) {
                        if (enemy.skills[i].talentName == Talent.TalentName.Mine) {
                            playerAtTrigger.GetComponent<SpriteRenderer>().sprite = enemy.skills[i].sprite;
                        }
                    }
                }
                playerAtTrigger.SetActive(true);
                UsedSkill();
            }
        }
    }

    void BuildTiles(GameObject go) {
        GameManager.instance.newGridSize = GameManager.instance.gridSize + 1;
        if (player.state == Player.State.Playing) {
            for (int i = 0; i < player.skills.Count; i++) {
                if (player.skills[i].talentName == Talent.TalentName.BuildTiles) {
                    GridManager.instance.ChangeGridSize(GameManager.instance.gridSize, GameManager.instance.newGridSize, player.skills[i].direction);
                }
            }
            UsedSkill();
        } else if (enemy.state == Player.State.Playing) {
            for (int i = 0; i < enemy.skills.Count; i++) {
                if (enemy.skills[i].talentName == Talent.TalentName.BuildTiles) {
                    GridManager.instance.ChangeGridSize(GameManager.instance.gridSize, GameManager.instance.newGridSize, enemy.skills[i].direction);
                }
            }
            UsedSkill();
        }
    }
    void DestroyTiles(GameObject go) {
        GameManager.instance.newGridSize = GameManager.instance.gridSize - 1;
        if (player.state == Player.State.Playing) {
            for (int i = 0; i < player.skills.Count; i++) {
                if (player.skills[i].talentName == Talent.TalentName.BuildTiles) {
                    GridManager.instance.ChangeGridSize(GameManager.instance.gridSize, GameManager.instance.newGridSize, player.skills[i].direction);
                }
            }
            UsedSkill();
        } else if (enemy.state == Player.State.Playing) {
            for (int i = 0; i < enemy.skills.Count; i++) {
                if (enemy.skills[i].talentName == Talent.TalentName.BuildTiles) {
                    GridManager.instance.ChangeGridSize(GameManager.instance.gridSize, GameManager.instance.newGridSize, enemy.skills[i].direction);
                }
            }
            UsedSkill();
        }
    }

    // Passives

    // Talent Logic
    public void AddTalent(Player _player, Talent.TalentName talentName) {
        _player.skills.Add(new Talent(talentName));
        if (_player == player) {
            _player.skills.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Player/" + talentName.ToString());
        } else {
            _player.skills.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Enemy/" + talentName.ToString());
        }
        _player.skills.Last().SetTalentObject();
        _player.skills.Last().cooldown = _player.initialCooldown;
    }
    public void SetSkills() {
        for (int i = 0; i < player.skills.Count; i++) {
            GameObject skill = Instantiate(Resources.Load<GameObject>("Prefabs/Talents/Actives/TALENT"), skills.transform);
            skill.name = player.skills[i].talentName.ToString();
            skill.GetComponent<Image>().sprite = player.skills[i].sprite;
            Talent talent = player.skills[i];
            skill.GetComponent<Button>().onClick.AddListener(delegate{SelectSkill(talent);});
        }
    }
    public void SelectSkill(Talent skill) {
        confirmSkillMenu.SetActive(false);
        if (player.state == Player.State.Playing) {
            confirmSkillMenu.GetComponent<SetConfirmSkillMenu>().talentObject = skill.talentObject;
            player.state = Player.State.SelectingSkill;
        } else if (enemy.state != Player.State.Playing) {
            confirmSkillMenu.GetComponent<SetConfirmSkillMenu>().talentObject = skill.talentObject;
            enemy.state = Player.State.SelectingSkill;
        }
        confirmSkillMenu.SetActive(true);
        confirmSkillMenu.transform.GetChild(1).transform.localScale = new Vector3(0, 0, 0);
        confirmSkillMenu.transform.localPosition = new Vector3(0, 0, 0);
        LeanTween.scale(confirmSkillMenu.transform.GetChild(1).gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic();
    }
    public void ConfirmSkill(Talent skill) {
        if (skill.cooldown == 0) {
            if (player.state == Player.State.SelectingSkill && !player.skillUsed) {
                player.activeSkill = skill;
                player.state = Player.State.UsingSkill;
                skillMenu.transform.localPosition = new Vector3(0, 3840, 0);
                selectSkillMenu.SetActive(false);
                selectSkillMenu.SetActive(true);
            } else if (enemy.state == Player.State.SelectingSkill && !enemy.skillUsed) {
                enemy.activeSkill = skill;
                enemy.state = Player.State.UsingSkill;
                skillMenu.transform.localPosition = new Vector3(0, 3840, 0);
                selectSkillMenu.SetActive(false);
                selectSkillMenu.SetActive(true);
            }
        }
    }
    public void CancelSkill() {
        confirmSkillMenu.transform.localPosition = new Vector3(0, -3840, 0);
        skillMenu.transform.localPosition = Vector3.zero;
        if (player.state != Player.State.Inactive) {
            player.state = Player.State.Playing;
        } else if (enemy.state != Player.State.Inactive) {
            enemy.state = Player.State.Playing;
        }
    }
    public void UsedSkill() {
        confirmSkillMenu.transform.localPosition = new Vector3(0, -3840, 0);
        skillMenu.transform.localPosition = Vector3.zero;
        if (player.state != Player.State.Inactive) {
            player.state = Player.State.Playing;
            player.UsedSkill();
        } else if (enemy.state != Player.State.Inactive) {
            enemy.state = Player.State.Playing;
            enemy.UsedSkill();
        }
    }

    void TriggerMine(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        // Bombs all tiles in a box shape
        for (int i = (x - 1); i < (x + 2); i++) {
            for (int n = (y - 1); n < (y + 2); n++) {
                if (i >= 0 && n >= 0 && i < GameManager.instance.gridSize && n < GameManager.instance.gridSize) {
                    GameObject tile = GridManager.instance.Tiles[i][n];
                    if (tile.tag.Contains("Mine") && (i != x && n != y)) {
                        TriggerMine(tile);
                    }
                    BombTrigger(tile);
                    tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    tile.transform.GetChild(0).gameObject.SetActive(false);
                    tile.tag = "Untagged";
                }
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
