using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildGrid : MonoBehaviour {
    public GameObject[] tiles;
    public GameObject grid;
    public string[][] gridReference;

    public void BuildTheGrid() {
        int size = GameManager.instance.gridSize;
        gridReference = new string[7][];
        for (int i = 0; i < 7; i++) {
            gridReference[i] = new string[7];
        }
        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                gridReference[x][y] = "Tile " + x + "," + y;
            }
        }
        bool[] modifications = { true, true };
        UpdateGrid(0, size, modifications);
    }
    void FormatTiles(GameObject tile, int size, int x, int y) {
        UpdateTileSprites(tile, size, x, y);
        float center = (size / 2);
        tile.transform.GetChild(0).name = x + "," + y;
        tile.transform.localScale = Vector3.one;
        tile.transform.SetSiblingIndex(grid.transform.childCount - 1);
        tile.transform.position = new Vector3((x - center) * 2.35f, (y - center) * 2.35f, 0);
        if (size % 2 == 0) {
            tile.transform.position += new Vector3(1.175f, 1.175f, 0);
        }
    }
    public void UpdateTileSprites(GameObject tile, float size, int x, int y) {
        // Bottom Left
        if (x == 0 && y == 0 && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[0].GetComponent<SpriteRenderer>().sprite;
        }
        // Bottom Middle
        else if (x < (size - 1) && y == 0 && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[1].GetComponent<SpriteRenderer>().sprite;
        }
        // Bottom Right
        else if (x == (size - 1) && y == 0 && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[2].GetComponent<SpriteRenderer>().sprite;
        }
        // Middle Left
        else if (x == 0 && y < (size - 1) && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[3].GetComponent<SpriteRenderer>().sprite;
        }
        // Middle Middle
        else if ((x < (size - 1) && y < (size - 1)) || size == 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[4].GetComponent<SpriteRenderer>().sprite;
        }
        // Middle Right
        else if (x == (size - 1) && y < (size - 1) && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[5].GetComponent<SpriteRenderer>().sprite;
        }
        // Top Left
        else if (x == 0 && y == (size - 1) && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[6].GetComponent<SpriteRenderer>().sprite;
        }
        // Top Middle
        else if (x < (size - 1) && y == (size - 1) && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[7].GetComponent<SpriteRenderer>().sprite;
        }
        // Top Right
        else if (x == (size - 1) && y == (size - 1) && size > 1) {
            tile.GetComponent<SpriteRenderer>().sprite = tiles[8].GetComponent<SpriteRenderer>().sprite;
        }
    }
    void IncreaseGridSize(int gridSize, bool[] gridModification) {
        // Build the new column based on modification
        // Builds to the right
        GameObject[] tilesToRename = new GameObject[gridSize];
        if (gridModification[0]) {
            // Middle Right
            for (int y = 0; y < gridSize; y++) {
                GameObject tile = Instantiate(tiles[5], grid.transform);
                tilesToRename[y] = tile;
            }
        // Builds to the left
        } else {
            for (int x = gridSize; x > 0; x--) {
                for (int y = gridSize; y > 0; y--) {
                    GameObject.Find(gridReference[x - 1][y - 1]).name = gridReference[x][y - 1];
                }
            }
            for (int y = 0; y < gridSize; y++) {
                // Middle Left
                GameObject tile = Instantiate(tiles[3], grid.transform);
                tilesToRename[y] = tile;
            }
        }
        // Build the new row based on modification
        // Builds top
        if (gridModification[1]) {
            if (gridModification[0]) {
                for (int y = 0; y < gridSize; y++) {
                    tilesToRename[y].name = gridReference[gridSize][y];
                }
            } else {
                for (int y = 0; y < gridSize; y++) {
                    tilesToRename[y].name = gridReference[0][y];
                }
            }
            for (int x = 0; x < gridSize + 1; x++) {
                GameObject tile;
                // Top Left
                if (x == 0) {
                    tile = Instantiate(tiles[6], grid.transform);
                }
                // Top Middle
                else if (x < gridSize) {
                    tile = Instantiate(tiles[7], grid.transform);
                }
                // Top Right
                else {
                    tile = Instantiate(tiles[8], grid.transform);
                }
                tile.name = gridReference[x][gridSize];
            }
        // Builds bottom
        } else {
            if (gridModification[0]) {
                for (int y = 0; y < gridSize; y++) {
                    tilesToRename[y].name = gridReference[gridSize][y + 1];
                }
                for (int y = gridSize; y > 0; y--) {
                    for (int x = gridSize; x > 0; x--) {
                        GameObject.Find(gridReference[x - 1][y - 1]).name = gridReference[x - 1][y];
                    }
                }
                for (int x = 0; x < gridSize + 1; x++) {
                    GameObject tile;
                    // Bottom Left
                    if (x == 0) {
                        tile = Instantiate(tiles[0], grid.transform);
                    }
                    // Bottom Middle
                    else if (x < gridSize) {
                        tile = Instantiate(tiles[1], grid.transform);
                    }
                    // Bottom Right
                    else {
                        tile = Instantiate(tiles[2], grid.transform);
                    }
                    tile.name = gridReference[x][0];
                }
            } else {
                for (int y = 0; y < gridSize; y++) {
                    tilesToRename[y].name = gridReference[0][y + 1];
                }
                for (int y = gridSize; y > 0; y--) {
                    for (int x = gridSize; x > 0; x--) {
                        GameObject.Find(gridReference[x][y - 1]).name = gridReference[x][y];
                    }
                }
                for (int x = 0; x < gridSize + 1; x++) {
                    GameObject tile;
                    // Bottom Left
                    if (x == 0) {
                        tile = Instantiate(tiles[0], grid.transform);
                    }
                    // Bottom Middle
                    else if (x < gridSize) {
                        tile = Instantiate(tiles[1], grid.transform);
                    }
                    // Bottom Right
                    else {
                        tile = Instantiate(tiles[2], grid.transform);
                    }
                    tile.name = gridReference[x][0];
                }
            }
        }
    }
    void DecreaseGridSize(int gridSize, int newGridSize, bool[] gridModification) {
        int amount = grid.transform.childCount;
        GameObject[] currentTiles = new GameObject[amount];
        bool[] excludedTiles = new bool[amount];
        for (int i = 0; i < amount; i++) {
            currentTiles[i] = grid.transform.GetChild(i).gameObject; 
        }
        for (int i = 0; i < amount; i++) {
            Destroy(grid.transform.GetChild(i).gameObject);
        }
        while (newGridSize < gridSize) {
            // Determines the columns to destroy based on modification
            if (gridModification[0]) {
                if (gridModification[1]) { // Excludes the right and top
                    for (int i = 0; i < amount; i++) {
                        //int x = tileCount / (gridSize - 1), y = tileCount % (gridSize - 1);
                        if (i >= (gridSize * (gridSize - 1)) || (i % gridSize) == (gridSize - 1)) {
                            excludedTiles[i] = true;
                        } /*else {
                            GameObject tile = Instantiate(currentTiles[i], grid.transform);
                            tile.name = gridReference[x][y];
                            FormatTiles(tile, gridSize - 1, x, y);
                            tileCount++;
                        }*/
                    }
                } else { // Excludes the right and bottom
                    for (int i = 0; i < amount; i++) {
                        if (i >= (gridSize * (gridSize - 1)) || (i % gridSize) == 0) {
                            excludedTiles[i] = true;
                        }
                    }
                }
            } else {
                if (gridModification[1]) { // Destroys the left and top
                    for (int i = 0; i < amount; i++) {
                        if (i < gridSize || (i % gridSize) == (gridSize - 1)) {
                            excludedTiles[i] = true;
                        }
                    }
                } else { // Destroys the left and bottom
                    for (int i = 0; i < amount; i++) {
                        if (i < gridSize || (i % gridSize) == 0) {
                            excludedTiles[i] = true;
                        }
                    }
                }
            }
            gridSize--;
        }
        // Builds the non-excluded tiles
        int tileCount = 0;
        for (int i = 0; i < amount; i++) {
            int x = tileCount / (newGridSize), y = tileCount % (newGridSize);
            if (!excludedTiles[i] && x < 7 && y < 7) {
                GameObject tile = Instantiate(currentTiles[i], grid.transform);
                tile.name = gridReference[x][y];
                tile.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
                FormatTiles(tile, newGridSize, x, y);
                tileCount++;
            }
        }
    }
    public void UpdateGrid(int gridSize, int newGridSize, bool[] gridModification) {
        GameManager.instance.updatingGrid = true;
        grid.transform.localScale = Vector3.one;
        if (newGridSize > gridSize) {
            while (gridSize != newGridSize) {
                IncreaseGridSize(gridSize, gridModification);
                gridSize++;
            }
        } else {
            DecreaseGridSize(gridSize, newGridSize, gridModification);
        }

        for (int x = 0; x < newGridSize; x++) {
            for (int y = 0; y < newGridSize; y++) {
                FormatTiles(GameObject.Find(gridReference[x][y]), newGridSize, x, y);
            }
        }

        if (newGridSize > 1) {
            grid.transform.localScale = new Vector3(0.4f / Mathf.Log10(newGridSize), 0.4f / Mathf.Log10(newGridSize), 1);
        } else if (newGridSize == 1) {
            grid.transform.localScale = Vector3.one;
        }
        GameManager.instance.gridSize = GameManager.instance.newGridSize;
        GameManager.instance.updatingGrid = false;
    }
}
