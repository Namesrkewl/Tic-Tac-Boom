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
        DestroyTiles
    }

    public Direction direction;
    public enum Direction {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public int maxCooldown, scaling, cooldown, duration;
    public Sprite sprite;
    public string description;
    new public string name;

    public Talent(TalentName _talentName) {
        InitializeTalent(_talentName);
    }

    public void InitializeTalent(TalentName _talentName) {
        switch (_talentName) {
            case TalentName.SmallBomb:
                type = Type.Bomb;
                talentName = TalentName.SmallBomb;
                direction = Direction.None;
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
                direction = Direction.None;
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
                direction = Direction.None;
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
                direction = Direction.None;
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
                direction = Direction.None;
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
                direction = Direction.None;
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
}
