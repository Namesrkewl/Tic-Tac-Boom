using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangePlayers : MonoBehaviour
{
    /*
    GameManager go;
    public GameObject rebel, king, enemyRebel, enemyKing, basilisk, common, dragon, engineer, fragmented, knight, nimble, peasant, thief, vagabond, wizard, wraith;

    public void SetStoryModePlayers() {
        go = GameManager.instance;
        GameObject player = null;
        // Clear the previous player objects
        ClearPlayers();

        // Set Player
        if (go.player == "REBEL") {
            player = Instantiate(rebel, go.HUD.transform);
            player.transform.SetSiblingIndex(1);
            player.name = "Player";
        } else if (go.player == "KING") {
            player = Instantiate(king, go.HUD.transform);
            player.transform.SetSiblingIndex(1);
            player.name = "Player";
        }

        // Set Opponent
        ChangeStoryModeOpponent(player);
    }

    public void ChangeStoryModeOpponent(GameObject player) {
        GameObject opponent;
        // EXILED GAMEPLAY
        if (player.GetComponent<Player>().IsExiled) {
            if (go.opponent == "BASILISK") {
                opponent = Instantiate(basilisk, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "COMMON") {
                opponent = Instantiate(common, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "DRAGON") {
                opponent = Instantiate(dragon, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "ENGINEER") {
                opponent = Instantiate(engineer, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "FRAGMENTED") {
                opponent = Instantiate(fragmented, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "KING") {
                opponent = Instantiate(enemyKing, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "KNIGHT") {
                opponent = Instantiate(knight, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "NIMBLE") {
                opponent = Instantiate(nimble, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "PEASANT") {
                opponent = Instantiate(peasant, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "THIEF") {
                opponent = Instantiate(thief, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "VAGABOND") {
                opponent = Instantiate(vagabond, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "WIZARD") {
                opponent = Instantiate(wizard, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "WRAITH") {
                opponent = Instantiate(wraith, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "REBEL") {
                opponent = Instantiate(enemyRebel, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            }
        }
        // PURE GAMEPLAY
        else if (!player.GetComponent<Player>().IsExiled) {
            if (go.opponent == "BASILISK") {
                opponent = Instantiate(basilisk, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "COMMON") {
                opponent = Instantiate(common, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "DRAGON") {
                opponent = Instantiate(dragon, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "ENGINEER") {
                opponent = Instantiate(engineer, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "FRAGMENTED") {
                opponent = Instantiate(fragmented, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "REBEL") {
                opponent = Instantiate(enemyRebel, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "KNIGHT") {
                opponent = Instantiate(knight, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "NIMBLE") {
                opponent = Instantiate(nimble, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "PEASANT") {
                opponent = Instantiate(peasant, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "THIEF") {
                opponent = Instantiate(thief, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "VAGABOND") {
                opponent = Instantiate(vagabond, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "WIZARD") {
                opponent = Instantiate(wizard, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "WRAITH") {
                opponent = Instantiate(wraith, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            } else if (go.opponent == "KING") {
                opponent = Instantiate(enemyKing, go.HUD.transform);
                opponent.transform.SetSiblingIndex(2);
                opponent.name = "Enemy";
            }
        }
    }


    public void ClearPlayers() {
        go = GameManager.instance;
        // Clear the previous player objects
        for (int i = 1; i < go.HUD.transform.childCount; i++) {
            Destroy(go.HUD.transform.GetChild(i).gameObject);
        }
    }
    */
}
