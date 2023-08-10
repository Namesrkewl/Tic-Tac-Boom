using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeAI : MonoBehaviour
{

    protected List<GameObject> bestMoves;
    protected int[][] tileValues;
    public int blockedValue = -50;
    public int enemyValue = -5;
    public int baseValue = 1;
    public int playerValue = 20;
    public int impendingVictoryValue = 999;
    public int impendingDoomValue = 9999;
    public int victoryValue = 99999;
    public int startingGridSize = 0;
    public bool firstMove = true;

    protected virtual void Start() {
        if (gameObject != GameManager.instance) {
            //SetAI();
        }
    }

    public virtual GameObject AIMove() {
        // Single Move AI Based on the best move by point value
        ParseTileValues(PlayerManager.instance.enemy.remainingMoves);
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

    protected virtual void ParseTileValues(int moves) {
        int size = StoryManager.instance.gridSize;
        int originalMoves = moves;
        //blockedValue = playerValue * size;
        tileValues = new int[size][];
        for (int i = 0; i < size; i++) {
            tileValues[i] = new int[size];
        }
        bestMoves = new List<GameObject>();
        int highestValue = -999;

        for (int x = 0; x < size; x++) {    
            for (int y = 0; y < size; y++) {
                bool isVictory = true, blocking = false, blocked = false;
                int enemyTiles = 0, playerTiles = 0;
                int value = 0;
                // Check if tile is occupied
                GameObject tile = GridManager.instance.Tiles[x][y];
                if (tile.transform.GetChild(0).gameObject.activeSelf || tile.CompareTag("Wall")) {
                    continue;
                }
                if (x == y && (x + y) == (size - 1)) {
                    // MIDDLE TILE OF AN ODD SIZED GRID
                    // Value Left To Right Diagonal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[i][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++;
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0;
                    playerTiles = 0;
                    blocking = false;
                    blocked = false;
                    // Value Right to Left Diagonal
                    for (int i = size - 1; i >= 0; i--) {
                        int n = (size - 1) - i;
                        GameObject connectedTile = GridManager.instance.Tiles[n][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++;
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0;
                    playerTiles = 0;
                    blocking = false;
                    blocked = false;
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[x][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++;
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[i][y];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++; 
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }                       
                } else if (x == y) {
                    // TILE ON LEFT TO RIGHT DIAGONAL
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Left To Right Diagonal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[i][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++;
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0;
                    playerTiles = 0;
                    blocking = false;
                    blocked = false;
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[x][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++; 
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[i][y];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++; 
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                } else if ((x + y) == size - 1) {
                    // TILE ON RIGHT TO LEFT DIAGONAL
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Right to Left Diagonal
                    for (int i = size - 1; i >= 0; i--) {
                        int n = (size - 1) - i;
                        GameObject connectedTile = GridManager.instance.Tiles[n][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++;
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0;
                    playerTiles = 0;
                    blocking = false;
                    blocked = false;
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[x][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++; 
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0; 
                    playerTiles = 0;
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[i][y];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++; 
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }                    
                } else {
                    // TILE OUTSIDE OF DIAGONALS
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Vertical
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[x][i];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++; 
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
                        value += impendingVictoryValue;
                    } else if (blocked) {
                        value += blockedValue;
                    }
                    moves = originalMoves;
                    isVictory = true;
                    enemyTiles = 0; 
                    playerTiles = 0; 
                    blocking = false;
                    blocked = false;
                    // Value Horizontal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GridManager.instance.Tiles[i][y];
                        if (connectedTile.CompareTag("Enemy")) {
                            enemyTiles++; 
                            blocking = true;
                            value += playerValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += enemyValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += baseValue;
                        }
                    }
                    if (isVictory) {
                        value += victoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += impendingDoomValue;
                    } else if (enemyTiles >= (size / 2.0) && !blocked) {
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

    /*
    public virtual void SetAI() {
        if (gameObject != GameManager.instance.gameObject) {
            if (GameManager.instance.gameObject.GetComponent<StoryModeAI>()) {
                Destroy(GameManager.instance.gameObject.GetComponent<StoryModeAI>());
                GameManager.instance.gameObject.AddComponent<StoryModeAI>();
                // Sets Logic
                GameManager.instance.storyModeAI.blockedValue = blockedValue;
                GameManager.instance.storyModeAI.enemyValue = enemyValue;
                GameManager.instance.storyModeAI.baseValue = baseValue;
                GameManager.instance.storyModeAI.playerValue = playerValue;
                GameManager.instance.storyModeAI.impendingVictoryValue = impendingVictoryValue;
                GameManager.instance.storyModeAI.impendingDoomValue = impendingDoomValue;
                GameManager.instance.storyModeAI.victoryValue = gameObject.GetComponent<StoryModeAI>().victoryValue;
                // Sets Move Count
                GameManager.instance.enemyMoveMax = enemyMoveMax;
                // Sets Starting Grid Size
                GameManager.instance.newGridSize = startingGridSize;
            } else {
                GameManager.instance.gameObject.AddComponent<StoryModeAI>();
                // Sets Logic
                GameManager.instance.storyModeAI.blockedValue = blockedValue;
                GameManager.instance.storyModeAI.enemyValue = enemyValue;
                GameManager.instance.storyModeAI.baseValue = baseValue;
                GameManager.instance.storyModeAI.playerValue = playerValue;
                GameManager.instance.storyModeAI.impendingVictoryValue = impendingVictoryValue;
                GameManager.instance.storyModeAI.impendingDoomValue = impendingDoomValue;
                GameManager.instance.storyModeAI.victoryValue = gameObject.GetComponent<StoryModeAI>().victoryValue;
                // Sets Move Count
                GameManager.instance.enemyMoveMax = enemyMoveMax;
                // Sets Starting Grid Size
                GameManager.instance.newGridSize = startingGridSize;
            }
        }
    }
    */
}
