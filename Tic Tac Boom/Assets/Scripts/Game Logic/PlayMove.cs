using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayMove : MonoBehaviour {
    GameObject playerAtTrigger;
    [SerializeField] private AudioSource gameSounds;

    // Add the PlayerMove event to the EventManager at the on scene load
    private void Start() {
        EventManager.PlayerMoveEvent += PlayerMove;
    }

    // Player Movement logic at the selected tile (go)
    public void PlayerMove(GameObject go) {
        if (go == null) {
            return;
        }
        playerAtTrigger = go.transform.GetChild(0).gameObject;

        if (!playerAtTrigger.activeSelf && !GameManager.instance.bombInUse && !go.tag.Contains("Wall")) {
            if (GameManager.instance.isPlayerTurn) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                } else {
                    gameSounds.PlayOneShot(GameManager.instance.moveSound);
                    go.tag = "Player";
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = GameManager.instance.playerSprite;
                    playerAtTrigger.SetActive(true);
                }
                NextTurn();
            } else {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                } else {
                    gameSounds.PlayOneShot(GameManager.instance.moveSound);
                    go.tag = "Opponent";
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = GameManager.instance.opponentSprite;
                    playerAtTrigger.SetActive(true);
                }
                //NextTurn();
            }
        } else if (GameManager.instance.talents.usingSmallBomb) {
            SmallBomb(go);
            if (GameManager.instance.isPlayerTurn) {
                /*GameManager.instance.playerBombCooldowns[0] += 1;
                GameManager.instance.playerBombCooldowns[0][0] = GameManager.instance.playerBombCooldowns[0][1];*/
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            } else {
                /*GameManager.instance.opponentBombCooldowns[0][1] += 1;
                GameManager.instance.opponentBombCooldowns[0][0] = GameManager.instance.opponentBombCooldowns[0][1]; */
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            }
        } else if (GameManager.instance.talents.usingCrossBomb) {
            CrossBomb(go);
            if (GameManager.instance.isPlayerTurn) {
                /*GameManager.instance.playerBombCooldowns[1][1] += 2;
                GameManager.instance.playerBombCooldowns[1][0] = GameManager.instance.playerBombCooldowns[1][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;*/
            } else {
                /*GameManager.instance.opponentBombCooldowns[1][1] += 2;
                GameManager.instance.opponentBombCooldowns[1][0] = GameManager.instance.opponentBombCooldowns[1][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;*/
            }
        } else if (GameManager.instance.talents.usingXBomb) {
            XBomb(go);
            if (GameManager.instance.isPlayerTurn) {
                /*GameManager.instance.playerBombCooldowns[2][1] += 2;
                GameManager.instance.playerBombCooldowns[2][0] = GameManager.instance.playerBombCooldowns[2][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;*/
            } else {
                /*GameManager.instance.opponentBombCooldowns[2][1] += 2;
                GameManager.instance.opponentBombCooldowns[2][0] = GameManager.instance.opponentBombCooldowns[2][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;*/
            }
        } else if (GameManager.instance.talents.usingMine && !go.tag.Contains("Wall") && !playerAtTrigger.activeSelf) {
            Mine(go);
            if (GameManager.instance.isPlayerTurn) {
                /*GameManager.instance.playerBombCooldowns[3][1] += 2;
                GameManager.instance.playerBombCooldowns[3][0] = GameManager.instance.playerBombCooldowns[3][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;*/
            } else {
                /*GameManager.instance.opponentBombCooldowns[3][1] += 2;
                GameManager.instance.opponentBombCooldowns[3][0] = GameManager.instance.opponentBombCooldowns[3][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;*/
            }
        } 
    }

    public void NextTurn() {
        GameManager.instance.turnCounter += 1;
    }

    // TALENTS

    // ACTIVES ABILITIES

    void SmallBomb(GameObject go) {
        playerAtTrigger.GetComponent<SpriteRenderer>().sprite = null;
        playerAtTrigger.SetActive(false);
        go.tag = "Untagged";
        GameManager.instance.talents.CancelSkill();
    }

    void CrossBomb(GameObject go) {

        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        if (GameObject.Find(x + "," + y) != null) {
            // Bombs the selected tile
            GameObject.Find(x + "," + y).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            GameObject.Find(x + "," + y).transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find(x + "," + y).tag = "Untagged";

            // Bombs surrounding tiles in a + shape
            if (GameObject.Find((x + 1) + "," + y) != null) {
                GameObject.Find((x + 1) + "," + y).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x + 1) + "," + y).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x + 1) + "," + y).tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + y) != null) {
                GameObject.Find((x - 1) + "," + y).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x - 1) + "," + y).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x - 1) + "," + y).tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y + 1)) != null) {
                GameObject.Find(x + "," + (y + 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find(x + "," + (y + 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find(x + "," + (y + 1)).tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y - 1)) != null) {
                GameObject.Find(x + "," + (y - 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find(x + "," + (y - 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find(x + "," + (y - 1)).tag = "Untagged";
            }
            GameManager.instance.talents.CancelSkill();
        }

    }

    void XBomb(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        if (GameObject.Find(x + "," + y) != null) {
            // Bombs the selected tile
            GameObject.Find(x + "," + y).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            GameObject.Find(x + "," + y).transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find(x + "," + y).tag = "Untagged";

            // Bombs surrounding tiles in a x shape
            if (GameObject.Find((x + 1) + "," + (y + 1)) != null) {
                GameObject.Find((x + 1) + "," + (y + 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x + 1) + "," + (y + 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x + 1) + "," + (y + 1)).tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y - 1)) != null) {
                GameObject.Find((x - 1) + "," + (y - 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x - 1) + "," + (y - 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x - 1) + "," + (y - 1)).tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y + 1)) != null) {
                GameObject.Find((x - 1) + "," + (y + 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x - 1) + "," + (y + 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x - 1) + "," + (y + 1)).tag = "Untagged";
            }
            if (GameObject.Find((x + 1) + "," + (y - 1)) != null) {
                GameObject.Find((x + 1) + "," + (y - 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x + 1) + "," + (y - 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x + 1) + "," + (y - 1)).tag = "Untagged";
            }
            GameManager.instance.talents.CancelSkill();
        }
    }

    void Mine(GameObject go) {
        go.tag = "Mine";
        GameManager.instance.talents.CancelSkill();
    }

    void TriggerMine(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        if (GameObject.Find(x + "," + y) != null) {
            // Bombs the selected tile
            GameObject.Find(x + "," + y).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            GameObject.Find(x + "," + y).transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find(x + "," + y).tag = "Untagged";

            // Bombs surrounding tiles in a x shape
            if (GameObject.Find((x + 1) + "," + (y + 1)) != null) {
                GameObject.Find((x + 1) + "," + (y + 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x + 1) + "," + (y + 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x + 1) + "," + (y + 1)).tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y - 1)) != null) {
                GameObject.Find((x - 1) + "," + (y - 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x - 1) + "," + (y - 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x - 1) + "," + (y - 1)).tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y + 1)) != null) {
                GameObject.Find((x - 1) + "," + (y + 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x - 1) + "," + (y + 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x - 1) + "," + (y + 1)).tag = "Untagged";
            }
            if (GameObject.Find((x + 1) + "," + (y - 1)) != null) {
                GameObject.Find((x + 1) + "," + (y - 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x + 1) + "," + (y - 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x + 1) + "," + (y - 1)).tag = "Untagged";
            }
            // Bombs surrounding tiles in a + shape
            if (GameObject.Find((x + 1) + "," + y) != null) {
                GameObject.Find((x + 1) + "," + y).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x + 1) + "," + y).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x + 1) + "," + y).tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + y) != null) {
                GameObject.Find((x - 1) + "," + y).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find((x - 1) + "," + y).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find((x - 1) + "," + y).tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y + 1)) != null) {
                GameObject.Find(x + "," + (y + 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find(x + "," + (y + 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find(x + "," + (y + 1)).tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y - 1)) != null) {
                GameObject.Find(x + "," + (y - 1)).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find(x + "," + (y - 1)).transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find(x + "," + (y - 1)).tag = "Untagged";
            }
        }
    }
}
