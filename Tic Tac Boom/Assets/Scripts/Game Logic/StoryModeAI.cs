using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeAI : MonoBehaviour
{
    private List<GameObject> bestMoves;
    private int[][] tileValues;
    [SerializeField] private int opponentValue = 20, playerValue = -5, baseValue = 1, blockedValue = -50, victoryValue = 9999, impendingVictoryValue = 200, impendingDoomValue = 1000;

    public GameObject AIMove() {

        // Single Move AI Based on the best move by point value
        ParseTileValues();
        if (bestMoves.Count != 0) {
            Debug.Log("The best moves are: ");
            for (int i = 0; i < bestMoves.Count; i++) {
                Debug.Log($"{bestMoves[i].name} ");
            }
            return bestMoves[Random.Range(0, (bestMoves.Count-1))];
        }
        return null;

        /* Completely Random AI
        do {
            int size = GameManager.instance.gridSize;
            gridX = Random.Range(0, (size));
            gridY = Random.Range(0, (size));
        }
        while (GameObject.Find($"{gridX},{gridY}").transform.GetChild(0).gameObject.activeSelf);
        return GameObject.Find($"{gridX},{gridY}");
        */
    }

    void ParseTileValues() {
        int size = GameManager.instance.gridSize;
        //blockedValue = opponentValue * size;
        tileValues = new int[size][];
        for (int i = 0; i < size; i++) {
            tileValues[i] = new int[size];
        }
        bestMoves = new List<GameObject>();
        int highestValue = -999;

        for (int x = 0; x < size; x++) {    
            for (int y = 0; y < size; y++) {
                bool isVictory = true, blocking = false, blocked = false;
                int opponentTiles = 0, playerTiles = 0;
                int value = 0;
                // Check if tile is occupied
                GameObject tile = GameObject.Find($"{x},{y}");
                if (tile.transform.GetChild(0).gameObject.activeSelf || tile.CompareTag("Wall")) {
                    continue;
                }
                if (x == y && (x + y) == (size - 1)) {
                    // MIDDLE TILE OF AN ODD SIZED GRID
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{x},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++;
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != y) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{i},{y}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != x) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Left To Right Diagonal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{i},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != x) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Right to Left Diagonal
                    for (int i = size - 1; i >= 0; i--) {
                        int n = (size - 1) - i;
                        GameObject connectedTile = GameObject.Find($"{n},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != y) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                } else if (x == y) {
                    // TILE ON LEFT TO RIGHT DIAGONAL
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{x},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != y) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{i},{y}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != x) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Left To Right Diagonal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{i},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != x) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                } else if ((x + y) == size - 1) {
                    // TILE ON RIGHT TO LEFT DIAGONAL
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{x},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != y) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0;
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{i},{y}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != x) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Right to Left Diagonal
                    for (int i = size - 1; i >= 0; i--) {
                        int n = (size - 1) - i;
                        GameObject connectedTile = GameObject.Find($"{n},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != y) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                } else {
                    // TILE OUTSIDE OF DIAGONALS
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{x},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != y) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    isVictory = true;
                    opponentTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{i},{y}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++; 
                            blocking = true;
                            value += opponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += playerValue;
                        } else {
                            if (i != x) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value = victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value = impendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                }
                tileValues[x][y] = value;
                if (value > highestValue) {
                    bestMoves.Clear();
                    bestMoves.Add(tile);
                    highestValue = value;
                } else if (value == highestValue) {
                    bestMoves.Add(tile);
                }
            }
        }
        Debug.Log($"The highest value is {highestValue}");

    }
}
