using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayMove : EventTrigger {
    GameObject button;
    GameObject playerAtButton;
    [SerializeField] private AudioSource gameSounds;

    public void PlayerMove() {
        button = EventSystem.current.currentSelectedGameObject;
        playerAtButton = button.transform.GetChild(0).gameObject;
        gameSounds.PlayOneShot(GameManager.instance.moveSound);

        if (!playerAtButton.activeSelf && !GameManager.instance.bombInUse && !button.tag.Contains("Wall")) {
            if (GameManager.instance.isPlayerTurn == true) {
                if (button.tag.Contains("Mine")) {
                    TriggerMine(button);
                } else {
                    button.tag = "Player";
                    playerAtButton.GetComponent<SpriteRenderer>().sprite = GameManager.instance.playerSprite;
                    playerAtButton.SetActive(true);
                }
                NextTurn();
            } else {
                if (button.tag.Contains("Mine")) {
                    TriggerMine(button);
                } else {
                    button.tag = "Opponent";
                    playerAtButton.GetComponent<SpriteRenderer>().sprite = GameManager.instance.opponentSprite;
                    playerAtButton.SetActive(true);
                }
                NextTurn();
            }
        } else if (GameManager.instance.talents.usingSmallBomb) {
            SmallBomb(button);
            if (GameManager.instance.isPlayerTurn) {
                GameManager.instance.playerBombCooldowns[0][1] += 1;
                GameManager.instance.playerBombCooldowns[0][0] = GameManager.instance.playerBombCooldowns[0][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            } else {
                GameManager.instance.opponentBombCooldowns[0][1] += 1;
                GameManager.instance.opponentBombCooldowns[0][0] = GameManager.instance.opponentBombCooldowns[0][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            }
        } else if (GameManager.instance.talents.usingCrossBomb) {
            CrossBomb(button);
            if (GameManager.instance.isPlayerTurn) {
                GameManager.instance.playerBombCooldowns[1][1] += 2;
                GameManager.instance.playerBombCooldowns[1][0] = GameManager.instance.playerBombCooldowns[1][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            } else {
                GameManager.instance.opponentBombCooldowns[1][1] += 2;
                GameManager.instance.opponentBombCooldowns[1][0] = GameManager.instance.opponentBombCooldowns[1][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            }
        } else if (GameManager.instance.talents.usingXBomb) {
            XBomb(button);
            if (GameManager.instance.isPlayerTurn) {
                GameManager.instance.playerBombCooldowns[2][1] += 2;
                GameManager.instance.playerBombCooldowns[2][0] = GameManager.instance.playerBombCooldowns[2][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            } else {
                GameManager.instance.opponentBombCooldowns[2][1] += 2;
                GameManager.instance.opponentBombCooldowns[2][0] = GameManager.instance.opponentBombCooldowns[2][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            }
        } else if (GameManager.instance.talents.usingMine && !button.tag.Contains("Wall") && !playerAtButton.activeSelf) {
            Mine(button);
            if (GameManager.instance.isPlayerTurn) {
                GameManager.instance.playerBombCooldowns[3][1] += 2;
                GameManager.instance.playerBombCooldowns[3][0] = GameManager.instance.playerBombCooldowns[3][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            } else {
                GameManager.instance.opponentBombCooldowns[3][1] += 2;
                GameManager.instance.opponentBombCooldowns[3][0] = GameManager.instance.opponentBombCooldowns[3][1];
                GameManager.instance.turnBombUsed = GameManager.instance.turnCounter;
            }
        } 
    }

    void NextTurn() {
        GameManager.instance.turnCounter += 1;
    }

    // TALENTS

    // ACTIVES ABILITIES

    void SmallBomb(GameObject button) {
        playerAtButton.GetComponent<SpriteRenderer>().sprite = null;
        playerAtButton.SetActive(false);
        button.tag = "Untagged";
        GameManager.instance.talents.CancelBombUse();
    }

    void CrossBomb(GameObject button) {

        string[] coordinates = button.name.Split(",");
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
            GameManager.instance.talents.CancelBombUse();
        }

    }

    void XBomb(GameObject button) {
        string[] coordinates = button.name.Split(",");
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
            GameManager.instance.talents.CancelBombUse();
        }
    }

    void Mine(GameObject button) {
        button.tag = "Mine";
        GameManager.instance.talents.CancelBombUse();
    }

    void TriggerMine(GameObject button) {
        string[] coordinates = button.name.Split(",");
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
    public void BuildTiles() {
        if (GameManager.instance.newGridSize > 0 && GameManager.instance.newGridSize < 8) {
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
        if (GameManager.instance.newGridSize > 0 && GameManager.instance.newGridSize < 8) {
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
}
