using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeAI : MonoBehaviour
{

    protected List<GameObject> bestMoves;
    protected int[][] tileValues;
    [field: SerializeField] public int BlockedValue { get; protected set; } = -50;
    [field: SerializeField] public int PlayerValue { get; protected set; } = -5;
    [field: SerializeField] public int BaseValue { get; protected set; } = 1;
    [field: SerializeField] public int OpponentValue { get; protected set; } = 20;
    [field: SerializeField] public int ImpendingVictoryValue { get; protected set; } = 999;
    [field: SerializeField] public int ImpendingDoomValue { get; protected set; } = 9999;
    [field: SerializeField] public int VictoryValue { get; protected set; } = 99999;
    [field: SerializeField] public int OpponentMoveMax { get; protected set; } = 0;
    [field: SerializeField] public int StartingGridSize { get; protected set; } = 0;
    [field: SerializeField] public int MoveCount { get; protected set; } = 0;
    [field: SerializeField] public bool FirstMove { get; protected set; } = true;
    protected virtual void Start() {
        if (gameObject != GameManager.instance) {
            SetAI();
        }
    }

    public virtual GameObject AIMove() {
        // Single Move AI Based on the best move by point value
        if (GameManager.instance.GetComponent<DragonAI>()) {
            return GameManager.instance.GetComponent<DragonAI>().AIMove();
        }
        ParseTileValues(GameManager.instance.opponentMoveCount);
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
        int size = GameManager.instance.gridSize;
        int originalMoves = moves;
        //BlockedValue = OpponentValue * size;
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
                    // Value Left To Right Diagonal
                    for (int i = 0; i < size; i++) {
                        GameObject connectedTile = GameObject.Find($"{i},{i}");
                        if (connectedTile.CompareTag("Opponent")) {
                            opponentTiles++;
                            blocking = true;
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }                       
                } else if (x == y) {
                    // TILE ON LEFT TO RIGHT DIAGONAL
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                } else if ((x + y) == size - 1) {
                    // TILE ON RIGHT TO LEFT DIAGONAL
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++;
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }                    
                } else {
                    // TILE OUTSIDE OF DIAGONALS
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
                    }
                    moves = originalMoves;
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
                            value += OpponentValue;
                        } else if (connectedTile.CompareTag("Player")) {
                            isVictory = false;
                            playerTiles++; 
                            blocked = true;
                            value += PlayerValue;
                        } else {
                            moves -= 1;
                            if (moves < 0) {
                                isVictory = false;
                            }
                            value += BaseValue;
                        }
                    }
                    if (isVictory) {
                        value += VictoryValue;
                    } else if (playerTiles >= (size / 2.0) && !blocking) {
                        value += ImpendingDoomValue;
                    } else if (opponentTiles >= (size / 2.0) && !blocked) {
                        value += ImpendingVictoryValue;
                    } else if (blocked) {
                        value += BlockedValue;
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

    public virtual void SetAI() {
        if (gameObject != GameManager.instance.gameObject) {
            if (GameManager.instance.gameObject.GetComponent<StoryModeAI>()) {
                Destroy(GameManager.instance.gameObject.GetComponent<StoryModeAI>());
                GameManager.instance.gameObject.AddComponent<StoryModeAI>();
                // Sets Logic
                GameManager.instance.storyModeAI.BlockedValue = BlockedValue;
                GameManager.instance.storyModeAI.PlayerValue = PlayerValue;
                GameManager.instance.storyModeAI.BaseValue = BaseValue;
                GameManager.instance.storyModeAI.OpponentValue = OpponentValue;
                GameManager.instance.storyModeAI.ImpendingVictoryValue = ImpendingVictoryValue;
                GameManager.instance.storyModeAI.ImpendingDoomValue = ImpendingDoomValue;
                GameManager.instance.storyModeAI.VictoryValue = gameObject.GetComponent<StoryModeAI>().VictoryValue;
                // Sets Move Count
                GameManager.instance.opponentMoveMax = OpponentMoveMax;
                // Sets Starting Grid Size
                GameManager.instance.newGridSize = StartingGridSize;
            } else {
                GameManager.instance.gameObject.AddComponent<StoryModeAI>();
                // Sets Logic
                GameManager.instance.storyModeAI.BlockedValue = BlockedValue;
                GameManager.instance.storyModeAI.PlayerValue = PlayerValue;
                GameManager.instance.storyModeAI.BaseValue = BaseValue;
                GameManager.instance.storyModeAI.OpponentValue = OpponentValue;
                GameManager.instance.storyModeAI.ImpendingVictoryValue = ImpendingVictoryValue;
                GameManager.instance.storyModeAI.ImpendingDoomValue = ImpendingDoomValue;
                GameManager.instance.storyModeAI.VictoryValue = gameObject.GetComponent<StoryModeAI>().VictoryValue;
                // Sets Move Count
                GameManager.instance.opponentMoveMax = OpponentMoveMax;
                // Sets Starting Grid Size
                GameManager.instance.newGridSize = StartingGridSize;
            }
        }
    }
}
