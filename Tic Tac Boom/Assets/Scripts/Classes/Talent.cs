using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent
{
    public Type type;
    public enum Type {
        Bomb,
        Skill,
        Passive
    }
    public TalentName talentName;
    public enum TalentName {
        SmallBomb,
        CrossBomb,
        XBomb,
        Mine,
        BuildTiles,
        DestroyTiles,
        FastStart,
        AdrenalineRush,
        QuickDraw,
        FastReload,
    }   

    public Direction direction;
    public enum Direction {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public int maxCooldown, scaling, cooldown, duration, baseMaxCooldown;
    public bool canAddTalent, canTriggerPassive;
    public Sprite sprite;
    public ParticleSystem particleSystem;
    public string name, description;
    public TalentObject talentObject;

    public Talent(TalentName _talentName) {
        InitializeTalent(_talentName);
    }

    public void InitializeTalent(TalentName _talentName) {
        switch (_talentName) {
            case TalentName.SmallBomb:
                type = Type.Bomb;
                talentName = TalentName.SmallBomb;
                maxCooldown = 3;
                baseMaxCooldown = maxCooldown;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                canAddTalent = true;
                sprite = Resources.Load<Sprite>("Skills/small_bomb");
                particleSystem = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PS_Explosion");
                name = "SMALL BOMB";
                description = $"DEPLOYS A BOMB THAT EXPLODES ON ONE TILE. DAMAGES WALLS.\n{maxCooldown}|{scaling}";
                break;
            case TalentName.CrossBomb:
                type = Type.Bomb;
                talentName = TalentName.CrossBomb;
                maxCooldown = 5;
                baseMaxCooldown = maxCooldown;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                canAddTalent = true;
                sprite = Resources.Load<Sprite>("Skills/cross_bomb");
                particleSystem = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PS_Explosion");
                name = "CROSS BOMB";
                description = $"EXPLODES ON THE SELECTED AND ADJACENT TILES. DAMAGES WALLS.\n{maxCooldown}|{scaling}";
                break;
            case TalentName.XBomb:
                type = Type.Bomb;
                talentName = TalentName.XBomb;
                maxCooldown = 5;
                baseMaxCooldown = maxCooldown;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                canAddTalent = true;
                sprite = Resources.Load<Sprite>("Skills/x_bomb");
                particleSystem = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PS_Explosion");
                name = "X BOMB";
                description = $"EXPLODES ON THE SELECTED AND DIAGONALLY CONNECTED TILES. DAMAGES WALLS.\n{maxCooldown}|{scaling}";
                break;
            case TalentName.Mine:
                type = Type.Bomb;
                talentName = TalentName.Mine;
                maxCooldown = 5;
                baseMaxCooldown = maxCooldown;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                canAddTalent = true;
                sprite = Resources.Load<Sprite>("Skills/mine");
                particleSystem = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PS_Explosion");
                name = "MINE";
                description = $"PLACE A MINE AT AN EMPTY TILE THAT EXPLODES IN A 3X3 RADIUS WHEN TRIGGERED. DESTROYS WALLS.\n{maxCooldown}|{scaling}";
                break;
            case TalentName.BuildTiles:
                type = Type.Skill;
                talentName = TalentName.BuildTiles;
                direction = Direction.None;
                maxCooldown = 5;
                baseMaxCooldown = maxCooldown;
                scaling = 3;
                duration = 0;
                cooldown = maxCooldown;
                canAddTalent = true;
                sprite = Resources.Load<Sprite>("Skills/build_tile");
                particleSystem = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PS_DustCloud");
                name = "BUILD TILES";
                description = $"EXPAND THE GRID BY 1 ROW AND COLUMN (3x3 -> 4x4).\n{maxCooldown}|{scaling}";
                break;
            case TalentName.DestroyTiles:
                type = Type.Skill;
                talentName = TalentName.DestroyTiles;
                direction = Direction.None;
                maxCooldown = 5;
                baseMaxCooldown = maxCooldown;
                scaling = 3;
                duration = 0;
                cooldown = maxCooldown;
                canAddTalent = true;
                sprite = Resources.Load<Sprite>("Skills/destroy_tile");
                particleSystem = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PS_DustCloud");
                name = "DESTROY TILES";
                description = $"DESTROYS 1 COLUMN AND 1 ROW OF THE GRID (3x3 -> 2x2). TAKES 1 MOVE COUNT TO USE.\n{maxCooldown}|{scaling}";
                break;
            case TalentName.FastStart:
                type = Type.Passive;
                talentName = TalentName.FastStart;
                canTriggerPassive = false;
                canAddTalent = true;
                sprite = null;
                name = "FAST START";
                description = $"Gain an extra move on your first turn.";
                break;
            case TalentName.AdrenalineRush:
                type = Type.Passive;
                talentName = TalentName.AdrenalineRush;
                canTriggerPassive = false;
                canAddTalent = true;
                sprite = null;
                name = "ADRENALINE RUSH";
                description = $"Every 3 turns, gain an extra move for that turn.";
                break;
            default:
                break;
        }
    }

    public void SetTalentObject() {
        talentObject.talent = this;
    }

    public void OnAddTalent() {
        switch(talentName) {
            case TalentName.SmallBomb:
                //maxCooldown = 10;
                break;
            default:
                break;
        }
    }

    public void Passive(Player _player) {
        switch(talentName) {
            case TalentName.FastStart:
                if (GameManager.instance.turn == 1 || GameManager.instance.turn == 2) {
                    canTriggerPassive = true;
                } else {
                    canTriggerPassive = false;
                }
                if (canTriggerPassive) {
                    _player.extraMoves += 1;
                }
                break;
            case TalentName.AdrenalineRush:
                if (GameManager.instance.round % 3 == 0) {
                    canTriggerPassive = true;
                } else {
                    canTriggerPassive = false;
                }
                if (canTriggerPassive) {
                    _player.extraMoves += 1;
                }
                break;
            default:
                break;
        }
    }

    public IEnumerator Skill(GameObject go) {
        switch (talentName) {
            case TalentName.BuildTiles:
                yield return BuildTiles(go);
                break;
            case TalentName.CrossBomb:
                yield return CrossBomb(go);
                break;
            case TalentName.DestroyTiles:
                yield return DestroyTiles(go);
                break;
            case TalentName.Mine:
                yield return Mine(go);
                break;
            case TalentName.SmallBomb:
                yield return SmallBomb(go);
                break;
            case TalentName.XBomb:
                yield return XBomb(go);
                break;
        }
        yield return null;
    }

    private IEnumerator SmallBomb(GameObject go) {
        ResolveSkill();
        if (go.tag.Contains("Mine")) {
            PlayerManager.instance.TriggerMine(go);
        }
        PlayerManager.instance.BombTrigger(go);
        go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
        go.transform.GetChild(0).gameObject.SetActive(false);
        go.tag = "Untagged";
        UsedSkill();
        yield return null;
    }

    private IEnumerator CrossBomb(GameObject go) {
        ResolveSkill();
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);
        GameObject tile = GridManager.instance.Tiles[x][y];
        if (tile.tag.Contains("Mine")) {
            PlayerManager.instance.TriggerMine(tile);
        }
        PlayerManager.instance.BombTrigger(tile);
        tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        tile.transform.GetChild(0).gameObject.SetActive(false);
        tile.tag = "Untagged";

        for (int i = (x - 1); i < (x + 2); i++) {
            if (i != x && i >= 0 && i < GameManager.instance.gridSize) {
                tile = GridManager.instance.Tiles[i][y];
                if (tile.tag.Contains("Mine")) {
                    PlayerManager.instance.TriggerMine(tile);
                }
                PlayerManager.instance.BombTrigger(tile);
                tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
        }
        for (int n = (y - 1); n < (y + 2); n++) {
            if (n != y && n >= 0 && n < GameManager.instance.gridSize) {
                tile = GridManager.instance.Tiles[x][n];
                if (tile.tag.Contains("Mine")) {
                    PlayerManager.instance.TriggerMine(go);
                }
                PlayerManager.instance.BombTrigger(tile);
                tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
        }
        UsedSkill();
        yield return null;
    }

    private IEnumerator XBomb(GameObject go) {
        ResolveSkill();
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);
        GameObject tile = GridManager.instance.Tiles[x][y];
        if (tile.tag.Contains("Mine")) {
            PlayerManager.instance.TriggerMine(tile);
        }
        PlayerManager.instance.BombTrigger(tile);
        tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        tile.transform.GetChild(0).gameObject.SetActive(false);
        tile.tag = "Untagged";

        for (int i = (x - 1); i < (x + 2); i++) {
            for (int n = (y - 1); n < (y + 2); n++) {
                if (i != x && n != y && (i >= 0 && n >= 0) && (i < GameManager.instance.gridSize && n < GameManager.instance.gridSize)) {
                    tile = GridManager.instance.Tiles[i][n];
                    if (tile.tag.Contains("Mine")) {
                        PlayerManager.instance.TriggerMine(tile);
                    }
                    PlayerManager.instance.BombTrigger(GridManager.instance.Tiles[i][n]);
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
                        PlayerManager.instance.TriggerMine(go);
                    }
                    PlayerManager.instance.BombTrigger(GridManager.instance.Tiles[i][n]);
                    tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                    tile.transform.GetChild(0).gameObject.SetActive(false);
                    tile.tag = "Untagged";
                }
            }
        }
        UsedSkill();
        yield return null;
    }

    private IEnumerator Mine(GameObject go) {
        if (go.tag.Contains("Mine")) {
            ResolveSkill();
            PlayerManager.instance.TriggerMine(go);
            UsedSkill();
        } else {
            if (!go.transform.GetChild(0).gameObject.activeSelf) {
                go.tag = "Mine";
                ResolveSkill();
                go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                go.transform.GetChild(0).gameObject.SetActive(true);
                UsedSkill();
            }
        }
        yield return null;
    }

    private IEnumerator BuildTiles(GameObject go) {
        if (go.tag.Contains("Skill")) {
            ResolveSkill();
            GameManager.instance.newGridSize = GameManager.instance.gridSize + 1;
            yield return GridManager.instance.ChangeGridSize(GameManager.instance.gridSize, GameManager.instance.newGridSize, go.GetComponent<DirectionInfo>().direction);
            UsedSkill();
        }
        yield return null;
    }
    private IEnumerator DestroyTiles(GameObject go) {
        if (go.tag.Contains("Skill")) {
            ResolveSkill();
            GameManager.instance.newGridSize = GameManager.instance.gridSize - 1;
            yield return GridManager.instance.ChangeGridSize(GameManager.instance.gridSize, GameManager.instance.newGridSize, go.GetComponent<DirectionInfo>().direction);
            UsedSkill();
        }
        yield return null;
    }

    public void ResolveSkill() {
        MenuManager.instance.confirmSkillMenu.transform.localPosition = new Vector3(0, -3840, 0);
        MenuManager.instance.skillMenu.transform.localPosition = Vector3.zero;
        MenuManager.instance.skillMenu.transform.GetChild(1).localPosition = new Vector3(0, -1400, 0);
        MenuManager.instance.skillMenu.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        MenuManager.instance.useSkillMenu.transform.GetChild(1).gameObject.SetActive(false);
        if (PlayerManager.instance.player.state != Player.State.Inactive) {
            PlayerManager.instance.player.state = Player.State.Idle;
        } else if (PlayerManager.instance.enemy.state != Player.State.Inactive) {
            PlayerManager.instance.enemy.state = Player.State.Idle;
        }
    }
    public void UsedSkill() {
        if (PlayerManager.instance.player.state == Player.State.Idle) {
            PlayerManager.instance.player.state = Player.State.Playing;
            PlayerManager.instance.player.UsedSkill();
        } else if (PlayerManager.instance.enemy.state == Player.State.Idle) {
            PlayerManager.instance.enemy.state = Player.State.Playing;
            PlayerManager.instance.enemy.UsedSkill();
        }
    }
}
