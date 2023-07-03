using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayMove : MonoBehaviour {
    GameObject playerAtTrigger;
    [SerializeField] private AudioSource gameSounds;
    [SerializeField] private ParticleSystem explosion, dustCloud;

    // Add the PlayerMove event to the EventManager at the on scene load
    private void Start() {
        EventManager.PlayerMoveEvent += StartPlayerMove;
    }

    public void StartPlayerMove(GameObject go) {
        StartCoroutine(PlayerMove(go));
    }

    // Player Movement logic at the selected tile (go)
    IEnumerator PlayerMove(GameObject go) {
        if (go == null) {
            yield return null;
        }
        playerAtTrigger = go.transform.GetChild(0).gameObject;

        if (!playerAtTrigger.activeSelf && !GameManager.instance.bombInUse && !go.tag.Contains("Wall")) {
            if (GameManager.instance.isPlayerTurn && GameManager.instance.playerMoveCount > 0) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                    GameManager.instance.playerMoveCount -= 1;
                } else {
                    ParticleSystem dust = Instantiate(dustCloud);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    gameSounds.Stop();
                    gameSounds.PlayOneShot(GameManager.instance.moveSound);
                    go.tag = "Player";
                    GameManager.instance.playerMoveCount -= 1;
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = GameManager.instance.playerSprite;
                    playerAtTrigger.SetActive(true);
                }
                if (GameManager.instance.playerMoveCount <= 0) {
                    if (!GameManager.instance.GameOver()) {
                        yield return new WaitForSeconds(1);
                        NextTurn();
                    }
                }
            } else if (!GameManager.instance.isPlayerTurn && GameManager.instance.opponentMoveCount > 0) {
                if (go.tag.Contains("Mine")) {
                    TriggerMine(go);
                    GameManager.instance.opponentMoveCount -= 1;
                } else {
                    ParticleSystem dust = Instantiate(dustCloud);
                    dust.transform.position = go.transform.position;
                    dust.Play();
                    gameSounds.PlayOneShot(GameManager.instance.moveSound);
                    go.tag = "Opponent";
                    GameManager.instance.opponentMoveCount -= 1;
                    playerAtTrigger.GetComponent<SpriteRenderer>().sprite = GameManager.instance.opponentSprite;
                    playerAtTrigger.SetActive(true);
                }
                if (GameManager.instance.opponentMoveCount <= 0) {
                    if (!GameManager.instance.GameOver()) {
                        yield return new WaitForSeconds(1);
                        NextTurn();
                    }
                }
            }
        } else if (GameManager.instance.usingSmallBomb) {
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
        } else if (GameManager.instance.usingCrossBomb) {
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
        } else if (GameManager.instance.usingXBomb) {
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
        } else if (GameManager.instance.usingMine && !go.tag.Contains("Wall") && !playerAtTrigger.activeSelf) {
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
        yield return null;
    }

    public void NextTurn() {
        GameManager.instance.turnCounter++;
        GameManager.instance.playerMoveCount = GameManager.instance.playerMoveMax;
        GameManager.instance.opponentMoveCount = GameManager.instance.opponentMoveMax;
    }

    // TALENTS

    // ACTIVES ABILITIES

    void SmallBomb(GameObject go) {
        if (go.tag.Contains("Mine")) {
            TriggerMine(go);
        }
        BombTrigger(go);
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
            if (go.tag.Contains("Mine")) {
                TriggerMine(go);
            }
            BombTrigger(go);
            go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.tag = "Untagged";

            // Bombs surrounding tiles in a + shape
            if (GameObject.Find((x + 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
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
            if (go.tag.Contains("Mine")) {
                TriggerMine(go);
            }
            BombTrigger(go);
            go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.tag = "Untagged";

            // Bombs surrounding tiles in a x shape
            if (GameObject.Find((x + 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x + 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            GameManager.instance.talents.CancelSkill();
        }
    }

    void Mine(GameObject go) {
        if (go.tag.Contains("Mine")) {
            TriggerMine(go);
            GameManager.instance.talents.CancelSkill();
        } else {
            go.tag = "Mine";
            go.GetComponent<SpriteRenderer>().sprite = GameManager.instance.mineSprite;
            go.transform.localScale = Vector3.zero;
            LeanTween.scale(go, Vector3.one, 0.5f).setEaseOutElastic();
            GameManager.instance.talents.CancelSkill();
        }
    }

    void TriggerMine(GameObject go) {
        string[] coordinates = go.name.Split(",");
        int x = Convert.ToInt32(coordinates[0]);
        int y = Convert.ToInt32(coordinates[1]);

        if (GameObject.Find(x + "," + y) != null) {
            // Bombs the selected tile
            BombTrigger(go);
            go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.tag = "Untagged";
            go.GetComponent<SpriteRenderer>().sprite = null;

            // Bombs surrounding tiles in a x shape
            if (GameObject.Find((x + 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x + 1) + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            // Bombs surrounding tiles in a + shape
            if (GameObject.Find((x + 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x + 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find((x - 1) + "," + y) != null) {
                GameObject tile = GameObject.Find((x - 1) + "," + y);
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y + 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y + 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
            if (GameObject.Find(x + "," + (y - 1)) != null) {
                GameObject tile = GameObject.Find(x + "," + (y - 1));
                if (tile.tag.Contains("Mine")) {
                    TriggerMine(tile);
                }
                BombTrigger(tile);
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                tile.transform.GetChild(0).gameObject.SetActive(false);
                tile.tag = "Untagged";
            }
        }
    }

    void BombTrigger(GameObject go) {
        ParticleSystem boom = Instantiate(explosion);
        boom.transform.position = go.transform.position;
        boom.Play();
        gameSounds.Stop();
        gameSounds.PlayOneShot(GameManager.instance.bombSound);
    }
}
