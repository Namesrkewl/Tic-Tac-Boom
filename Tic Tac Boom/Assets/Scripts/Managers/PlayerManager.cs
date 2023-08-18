using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player, enemy;
    public SetPlayerObject setPlayerObject, setEnemyObject;
    public List<Sprite> skins;
    public StoryModeAI storyModeAI;
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

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name == "StoryMode") {
            setPlayerObject = GameObject.Find("Player").GetComponent<SetPlayerObject>();
            setEnemyObject = GameObject.Find("Enemy").GetComponent<SetPlayerObject>();
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetPlayers(Player.Faction faction, Player.Character character, Player.Type type) {
        player.type = Player.Type.Player;
        player.faction = faction;
        player.character = character;
        if (player.faction == Player.Faction.Exiled) {
            player.initialCooldown = 3;
        } else {
            player.initialCooldown = 2;
        }
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
    private IEnumerator PlayerMove(GameObject go) {
        if (go == null) {
            yield return null;
        }
        if (!go.tag.Contains("Skill")) {
            playerAtTrigger = go.transform.GetChild(0).gameObject;
        } else {
            playerAtTrigger = null;
        }
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
            } else if (player.state == Player.State.UsingSkill || enemy.state == Player.State.UsingSkill) {
                yield return Skill(go);
            }
        } else if (playerAtTrigger != null) {
            if (!playerAtTrigger.activeSelf && !go.tag.Contains("Wall") && (player.state == Player.State.Playing || enemy.state == Player.State.Playing)) {
                if (player.state == Player.State.Playing && player.remainingMoves > 0) {
                    ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1]);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    AudioManager.instance.soundEffects.Stop();
                    AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/move_sfx"));
                    go.tag = "Player";
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = player.skin;
                    playerAtTrigger.SetActive(true);
                    player.remainingMoves -= 1;
                } else if (enemy.state == Player.State.Playing && enemy.remainingMoves > 0) {
                    ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1]);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/move_sfx"));
                    go.tag = "Enemy";
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = enemy.skin;
                    playerAtTrigger.SetActive(true);
                    enemy.remainingMoves -= 1;
                } 
            } else if (player.state == Player.State.UsingSkill || enemy.state == Player.State.UsingSkill) {
                yield return Skill(go);
            }
        } else if (player.state == Player.State.UsingSkill || enemy.state == Player.State.UsingSkill) {
            yield return Skill(go);
        }

        yield return null;
    }

    // SKILL USAGE

    private IEnumerator Skill(GameObject go) {
        if (player.state == Player.State.UsingSkill) {
            yield return player.activeSkill.Skill(go);
        } else if (enemy.state == Player.State.UsingSkill) {
            yield return enemy.activeSkill.Skill(go);
        }
        yield return null;
    }


    // Talent Logic

    public void GenerateTalents(Player _player, int amount) {
        if (_player.state == Player.State.AddingSkill) {
            GenerateSkills(_player, amount);
        } else if (_player.state == Player.State.AddingPassive) {
            GeneratePassives(_player, amount);
        }
    }
    private void GenerateSkills(Player _player, int amount) {
        GameObject choices = MenuManager.instance.skillChoices.transform.GetChild(1).GetChild(0).gameObject;
        for (int i = choices.transform.childCount - 1; i >= 0; i--) {
            Destroy(choices.transform.GetChild(i).gameObject);
        }
        if (_player.state == Player.State.AddingSkill) {
            _player.showingSkills.Clear();
            while (amount > 0 && _player.skillsPool.Count > 0) {
                int index = UnityEngine.Random.Range(0, _player.skillsPool.Count);
                _player.showingSkills.Add(_player.skillsPool[index]);
                _player.skillsPool.RemoveAt(index);
                amount--;
            }
            for (int i = _player.showingSkills.Count - 1; i >= 0; i--) {
                Talent skill = _player.showingSkills[i];
                if (_player == player) {
                    skill.talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Player/Skills/" + skill.talentName.ToString());
                } else {
                    skill.talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Enemy/Skills/" + skill.talentName.ToString());
                }
                skill.SetTalentObject();
                
                GameObject choice = Instantiate(Resources.Load<GameObject>("Prefabs/TalentChoices/Skill"), choices.transform);
                choice.name = skill.talentName.ToString();
                choice.transform.GetChild(0).GetComponent<Image>().sprite = skill.sprite;
                //choice.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { SelectSkill(skill); });
                _player.skillsPool.Add(skill);
            }
            _player.showingSkills.Clear();
        }
    }
    private void GeneratePassives(Player _player, int amount) {
        GameObject choices = MenuManager.instance.passiveChoices.transform.GetChild(1).GetChild(0).gameObject;
        for (int i = choices.transform.childCount - 1; i >= 0; i--) {
            Destroy(choices.transform.GetChild(i).gameObject);
        }
        if (_player.state == Player.State.AddingPassive) {
            _player.showingPassives.Clear();
            while (amount > 0 && _player.passivesPool.Count > 0) {
                int index = UnityEngine.Random.Range(0, _player.passivesPool.Count);
                _player.showingPassives.Add(player.passivesPool[index]);
                _player.passivesPool.RemoveAt(index);
                amount--;
            }
            for (int i = _player.showingPassives.Count - 1; i >= 0; i--) {
                Talent passive = _player.showingPassives[i];
                if (_player == player) {
                    passive.talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Player/Skills/" + passive.talentName.ToString());
                } else {
                    passive.talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Enemy/Skills/" + passive.talentName.ToString());
                }
                passive.SetTalentObject();
                GameObject choice = Instantiate(Resources.Load<GameObject>("Prefabs/TalentChoices/Passive"), choices.transform);
                choice.name = passive.talentName.ToString();
                //choice.GetComponent<Button>().onClick.AddListener(delegate { SelectSkill(passive); });
                choice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = passive.description;
                _player.passivesPool.Add(passive);
            }
            _player.showingPassives.Clear();
        }
    }
    public void AddTalent(Player _player, Talent.TalentName talentName) {
        Talent talent = new Talent(talentName);
        if (talent.type != Talent.Type.Passive) {
            AddSkill(_player, talentName);
        } else {
            AddPassive(_player, talentName);
        }
    }
    private void AddSkill(Player _player, Talent.TalentName talentName) {
        if (_player.skillsPool != null) {
            for (int i = 0; i < _player.skillsPool.Count; i++) {
                if (talentName == _player.skillsPool[i].talentName) {
                    _player.skills.Add(new Talent(talentName));
                    _player.skillsPool.RemoveAt(i);
                    if (_player == player) {
                        _player.skills.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Player/Skills/" + talentName.ToString());
                    } else {
                        _player.skills.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Enemy/Skills/" + talentName.ToString());
                    }
                    _player.skills.Last().SetTalentObject();
                    _player.skills.Last().OnAddTalent(_player);
                    _player.skills.Last().cooldown = _player.initialCooldown;
                    break;
                }
            }
        }
    }
    private void AddPassive(Player _player, Talent.TalentName talentName) {
        if (_player.passivesPool != null) {
            for (int i = 0; i < _player.passivesPool.Count; i++) {
                if (talentName == _player.passivesPool[i].talentName) {
                    _player.passives.Add(new Talent(talentName));
                    _player.passivesPool.RemoveAt(i);
                    if (_player == player) {
                        _player.passives.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Player/Passives/" + talentName.ToString());
                    } else {
                        _player.passives.Last().talentObject = Resources.Load<TalentObject>("Prefabs/ScriptableObjects/Talents/Enemy/Passives/" + talentName.ToString());
                    }
                    _player.passives.Last().SetTalentObject();
                    _player.passives.Last().OnAddTalent(_player);
                    _player.passives.Last().cooldown = _player.initialCooldown;
                    break;
                }
            }
        }
    }
    public void SetTalents() {
        if (player.skills != null) {
            for (int i = 0; i < player.skills.Count; i++) {
                GameObject skill = Instantiate(Resources.Load<GameObject>("Prefabs/Talents/Skill"), MenuManager.instance.skills.transform);
                skill.name = player.skills[i].talentName.ToString();
                skill.GetComponent<Image>().sprite = player.skills[i].sprite;
                Talent talent = player.skills[i];
                skill.GetComponent<Button>().onClick.AddListener(delegate { SelectSkill(talent); });
            }
        }
    }
    public void SelectSkill(Talent skill) {
        MenuManager.instance.confirmSkillMenu.SetActive(false);
        if (player.state != Player.State.Inactive) {
            MenuManager.instance.confirmSkillMenu.GetComponent<SetConfirmSkillMenu>().talentObject = skill.talentObject;
            player.state = Player.State.SelectingSkill;
        } else if (enemy.state != Player.State.Inactive) {
            MenuManager.instance.confirmSkillMenu.GetComponent<SetConfirmSkillMenu>().talentObject = skill.talentObject;
            enemy.state = Player.State.SelectingSkill;
        }
        MenuManager.instance.confirmSkillMenu.SetActive(true);
        MenuManager.instance.confirmSkillMenu.transform.GetChild(1).transform.localScale = new Vector3(0, 0, 0);
        MenuManager.instance.confirmSkillMenu.transform.localPosition = new Vector3(0, 0, 0);
        LeanTween.scale(MenuManager.instance.confirmSkillMenu.transform.GetChild(1).gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic();
    }
    public void ConfirmSkill(Talent skill) {
        if (skill.talentName == Talent.TalentName.DestroyTiles && GameManager.instance.gridSize == GameManager.instance.minGridSize) {
            return;
        } else if (skill.talentName == Talent.TalentName.BuildTiles && GameManager.instance.gridSize == GameManager.instance.maxGridSize) {
            return;
        }
        if (skill.cooldown == 0) {
            if (player.state == Player.State.SelectingSkill && !player.skillUsed) {
                player.activeSkill = skill;
                player.state = Player.State.UsingSkill;
                MenuManager.instance.skillMenu.transform.localPosition = new Vector3(0, 3840, 0);
                MenuManager.instance.useSkillMenu.SetActive(false);
                MenuManager.instance.useSkillMenu.SetActive(true);
                if (player.activeSkill.talentName == Talent.TalentName.BuildTiles || player.activeSkill.talentName == Talent.TalentName.DestroyTiles) {
                    MenuManager.instance.useSkillMenu.transform.GetChild(1).gameObject.SetActive(true);
                } 
            } else if (enemy.state == Player.State.SelectingSkill && !enemy.skillUsed) {
                enemy.activeSkill = skill;
                enemy.state = Player.State.UsingSkill;
                MenuManager.instance.skillMenu.transform.localPosition = new Vector3(0, 3840, 0);
                MenuManager.instance.useSkillMenu.SetActive(false);
                MenuManager.instance.useSkillMenu.SetActive(true);
                if (enemy.activeSkill.talentName == Talent.TalentName.BuildTiles || enemy.activeSkill.talentName == Talent.TalentName.DestroyTiles) {
                    MenuManager.instance.useSkillMenu.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }
    public void CancelSkill() {
        MenuManager.instance.confirmSkillMenu.transform.localPosition = new Vector3(0, -3840, 0);
        MenuManager.instance.skillMenu.transform.localPosition = Vector3.zero;
        MenuManager.instance.skillMenu.transform.GetChild(1).localPosition = new Vector3(0, -1400, 0);
        MenuManager.instance.skillMenu.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        MenuManager.instance.useSkillMenu.transform.GetChild(1).gameObject.SetActive(false);
        if (player.state != Player.State.Inactive) {
            player.state = Player.State.Playing;
        } else if (enemy.state != Player.State.Inactive) {
            enemy.state = Player.State.Playing;
        }
    }

    public void TriggerMine(GameObject go) {
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

    public void BombTrigger(GameObject go) {
        ParticleSystem boom = Instantiate(ParticleSystemsManager.instance.particleSystems[0]);
        boom.transform.position = go.transform.position;
        boom.Play();
        AudioManager.instance.soundEffects.Stop();
        AudioManager.instance.soundEffects.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/Explosions/bomb_sfx"));
    }
}
