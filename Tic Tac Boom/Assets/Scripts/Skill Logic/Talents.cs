using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talents : MonoBehaviour
{
    // Bombs
    public bool usingSmallBomb, usingCrossBomb, usingXBomb, usingMine;
    [SerializeField] private GameObject skillMenu, confirmSkillMenu;

    public void SetUsingSmallBomb() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[0] == 0) {
                usingSmallBomb = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[0] == 0) {
                usingSmallBomb = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            }
        }
    }
    public void SetUsingCrossBomb() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[1] == 0) {
                usingCrossBomb = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[1] == 0) {
                usingCrossBomb = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            }
        }
    }

    public void SetUsingXBomb() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[2] == 0) {
                usingXBomb = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[2] == 0) {
                usingXBomb = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            }
        }
    }
    public void SetUsingMine() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[3] == 0) {
                usingMine = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[3] == 0) {
                usingMine = true;
                GameManager.instance.bombInUse = true;
                UseSkill();
            }
        }
    }

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
        skillMenu.transform.localPosition = new Vector3(0, 0, 0);
        GameManager.instance.talents.usingSmallBomb = false;
        GameManager.instance.talents.usingCrossBomb = false;
        GameManager.instance.talents.usingXBomb = false;
        GameManager.instance.talents.usingMine = false;
        GameManager.instance.bombInUse = false;
    }
}
