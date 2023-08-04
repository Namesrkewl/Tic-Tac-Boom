using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent : MonoBehaviour
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
        DestroyTiles
    }

    public int maxCooldown, scaling, cooldown, duration;
    public Sprite sprite;
    public string description;
    new public string name;

    public Talent(Talent _talent) {
        InitializeTalent(_talent);
    }

    public void InitializeTalent(Talent _talent) {
        switch (_talent.talentName) {
            case TalentName.SmallBomb:
                type = Type.Bomb;
                talentName = TalentName.SmallBomb;
                maxCooldown = 3;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                sprite = Resources.Load("Assets/Resources/Skills/small_bomb_active.png") as Sprite;
                name = "SMALL BOMB";
                description = "DEPLOYS A BOMB THAT EXPLODES ON ONE TILE. DAMAGES WALLS.";
                break;
            case TalentName.CrossBomb:
                type = Type.Bomb;
                talentName = TalentName.CrossBomb;
                maxCooldown = 5;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                sprite = Resources.Load("Assets/Resources/Skills/cross_bomb_active.png") as Sprite;
                name = "CROSS BOMB";
                description = "EXPLODES ON THE SELECTED AND ADJACENT TILES. DAMAGES WALLS.";
                break;
            case TalentName.XBomb:
                type = Type.Bomb;
                talentName = TalentName.XBomb;
                maxCooldown = 5;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                sprite = Resources.Load("Assets/Resources/Skills/x_bomb_active.png") as Sprite;
                name = "X BOMB";
                description = "EXPLODES ON THE SELECTED AND DIAGONALLY CONNECTED TILES. DAMAGES WALLS.";
                break;
            case TalentName.Mine:
                type = Type.Bomb;
                talentName = TalentName.Mine;
                maxCooldown = 5;
                scaling = 2;
                duration = 0;
                cooldown = maxCooldown;
                sprite = Resources.Load("Assets/Resources/Skills/mine breathing 3 4.png") as Sprite;
                name = "MINE";
                description = "EXPLODES ON ALL TILES IN A 3X3 RADIUS WHEN TRIGGERED. DESTROYS WALLS.";
                break;
            case TalentName.BuildTiles:
                type = Type.Skill;
                talentName = TalentName.BuildTiles;
                maxCooldown = 5;
                scaling = 3;
                duration = 0;
                cooldown = maxCooldown;
                sprite = Resources.Load("Assets/Resources/Skills/build_tile.png") as Sprite;
                name = "BUILD TILES";
                description = "EXPAND THE GRID BY 1 ROW AND COLUMN (3x3 -> 4x4).";
                break;
            case TalentName.DestroyTiles:
                type = Type.Skill;
                talentName = TalentName.DestroyTiles;
                maxCooldown = 5;
                scaling = 3;
                duration = 0;
                cooldown = maxCooldown;
                sprite = Resources.Load("Assets/Resources/Skills/destroy_tile.png") as Sprite;
                name = "DESTROY TILES";
                description = "DESTROYS 1 COLUMN AND 1 ROW OF THE GRID (3x3 -> 2x2). ENDS YOUR TURN.";
                break;
            default:
                break;
        }
    }

    // Bombs
    public GameObject skillMenu, confirmSkillMenu;

    // Skills
    public void BuildTiles() {
        if (GameManager.instance.newGridSize > 0 && GameManager.instance.newGridSize < 8 && GameManager.instance.playerBombCooldowns[4] == 0) {
            if (GameManager.instance.isPlayerTurn) {
                GameManager.instance.gridModification[0] = true;
                GameManager.instance.gridModification[1] = false;
                GameManager.instance.newGridSize = GameManager.instance.gridSize + 1;
                if (GameManager.instance.newGridSize == 8) GameManager.instance.newGridSize = 7;
            } else {
                GameManager.instance.gridModification[0] = true;
                GameManager.instance.gridModification[1] = true;
                GameManager.instance.newGridSize = GameManager.instance.gridSize + 1;
                if (GameManager.instance.newGridSize == 8) GameManager.instance.newGridSize = 7;
            }
        }
    }
    public void DestroyTiles() {
        if (GameManager.instance.newGridSize > 0 && GameManager.instance.newGridSize < 8 && GameManager.instance.playerBombCooldowns[5] == 0) {
            if (GameManager.instance.isPlayerTurn) {
                GameManager.instance.gridModification[0] = true;
                GameManager.instance.gridModification[1] = true;
                GameManager.instance.newGridSize = GameManager.instance.gridSize - 1;
                if (GameManager.instance.newGridSize == 0) GameManager.instance.newGridSize = 1;
            } else {
                GameManager.instance.gridModification[0] = false;
                GameManager.instance.gridModification[1] = false;
                GameManager.instance.newGridSize = GameManager.instance.gridSize - 1;
                if (GameManager.instance.newGridSize == 0) GameManager.instance.newGridSize = 1;
            }
        }
    }

    // Passives

    // Talent Logic
    public void UseSkill() {
        confirmSkillMenu.transform.GetChild(1).transform.localScale = new Vector3(0, 0, 0);
        confirmSkillMenu.transform.localPosition = new Vector3(0, 0, 0);
        LeanTween.scale(confirmSkillMenu.transform.GetChild(1).gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic();
    }
    public void ConfirmSkill() {
        skillMenu.transform.localPosition = new Vector3(0, 3840, 0);
    }
    public void CancelSkill() {
        confirmSkillMenu.transform.localPosition = new Vector3(0, -3840, 0);
        skillMenu.transform.localPosition = Vector3.zero;
        GameManager.instance.usingSmallBomb = false;
        GameManager.instance.usingCrossBomb = false;
        GameManager.instance.usingXBomb = false;
        GameManager.instance.usingMine = false;
        GameManager.instance.bombInUse = false;
    }
}
