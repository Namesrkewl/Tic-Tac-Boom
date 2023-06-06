using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talents : MonoBehaviour
{
    // Bombs
    public bool usingSmallBomb, usingCrossBomb, usingXBomb, usingMine;

    public void SetUsingSmallBomb() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[0][0] == 0) {
                usingSmallBomb = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.cancelSkillButton.SetActive(true);
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[0][0] == 0) {
                usingSmallBomb = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.cancelSkillButton.SetActive(true);
            }
        }
    }
    public void SetUsingCrossBomb() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[1][0] == 0) {
                usingCrossBomb = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.skillMenu.SetActive(false);
                GameManager.instance.cancelSkillButton.SetActive(true);
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[1][0] == 0) {
                usingCrossBomb = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.skillMenu.SetActive(false);
                GameManager.instance.cancelSkillButton.SetActive(true);
            }
        }
    }

    public void SetUsingXBomb() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[2][0] == 0) {
                usingXBomb = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.skillMenu.SetActive(false);
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[2][0] == 0) {
                usingXBomb = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.skillMenu.SetActive(false);
                GameManager.instance.cancelSkillButton.SetActive(true);
            }
        }
    }
    public void SetUsingMine() {
        if (!GameManager.instance.bombInUse && GameManager.instance.turnBombUsed != GameManager.instance.turnCounter) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerBombCooldowns[3][0] == 0) {
                usingMine = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.skillMenu.SetActive(false);
                GameManager.instance.cancelSkillButton.SetActive(true);
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentBombCooldowns[3][0] == 0) {
                usingMine = true;
                GameManager.instance.bombInUse = true;
                GameManager.instance.skillMenu.SetActive(false);
                GameManager.instance.cancelSkillButton.SetActive(true);
            }
        }
    }

    // Skills

    // Passives

    // Talent Logic
    public void UseSkill() {
        GameManager.instance.skillMenu.SetActive(true);
        GameManager.instance.useSkillButton.SetActive(false);
        GameManager.instance.cancelSkillButton.SetActive(true);
    }
    public void CancelBombUse() {
        if (GameManager.instance.bombInUse) {
            usingSmallBomb = false;
            usingCrossBomb = false;
            usingXBomb = false;
            usingMine = false;
            GameManager.instance.bombInUse = false;
            GameManager.instance.useSkillButton.SetActive(true);
            GameManager.instance.cancelSkillButton.SetActive(false);
        } else {
            GameManager.instance.skillMenu.SetActive(false);
            GameManager.instance.useSkillButton.SetActive(true);
            GameManager.instance.cancelSkillButton.SetActive(false);
        }
    }
}
