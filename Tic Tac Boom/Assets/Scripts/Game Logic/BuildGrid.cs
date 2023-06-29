using UnityEngine;
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
        float center = (size / 2);
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                GameObject tile;
                // Bottom Left
                if (x == 0 && y == 0 && size > 1) {
                    tile = Instantiate(tiles[0], new Vector3(0, 0), Quaternion.identity);
                }
                // Bottom Middle
                else if (x < (size - 1) && y == 0 && size > 1) {
                    tile = Instantiate(tiles[1], new Vector3(0, 0), Quaternion.identity);
                }
                // Bottom Right
                else if (x == (size - 1) && y == 0 && size > 1) {
                    tile = Instantiate(tiles[2], new Vector3(0, 0), Quaternion.identity);
                }
                // Middle Left
                else if (x == 0 && y < (size - 1) && size > 1) {
                    tile = Instantiate(tiles[3], new Vector3(0, 0), Quaternion.identity);
                }
                // Middle Middle
                else if ((x < (size - 1) && y < (size - 1)) || size == 1) {
                    tile = Instantiate(tiles[4], new Vector3(0, 0), Quaternion.identity);
                }
                // Middle Right
                else if (x == (size - 1) && y < (size - 1) && size > 1) {
                    tile = Instantiate(tiles[5], new Vector3(0, 0), Quaternion.identity);
                }
                // Top Left
                else if (x == 0 && y == (size - 1) && size > 1) {
                    tile = Instantiate(tiles[6], new Vector3(0, 0), Quaternion.identity);
                }
                // Top Middle
                else if (x < (size - 1) && y == (size - 1) && size > 1) {
                    tile = Instantiate(tiles[7], new Vector3(0, 0), Quaternion.identity);
                }
                // Top Right
                else {
                    tile = Instantiate(tiles[8], new Vector3(0, 0), Quaternion.identity);
                }
                tile.name = gridReference[x][y];
                FormatTiles(tile, center, size, x, y);
            }
        }
        if (size > 1) {
            grid.transform.localScale = new Vector3(0.4f / Mathf.Log10(size), 0.4f / Mathf.Log10(size), 1);
        } else if (size == 1) {
            grid.transform.localScale = Vector3.one;
        }
    }
    void FormatTiles(GameObject tile, float center, int size, int x, int y) {
        tile.transform.GetChild(0).name = x + "," + y;
        tile.transform.localScale = Vector3.one;
        tile.transform.SetParent(grid.transform);
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
    void IncreaseGridSize(int gridSize, int newGridSize, bool[] gridModification) {
        // Build the new column based on modification
        // Builds to the right
        GameObject[] tilesToRename = new GameObject[gridSize];
        if (gridModification[0]) {
            // Middle Right
            for (int y = 0; y < gridSize; y++) {
                GameObject tile = Instantiate(tiles[5], new Vector3(0, 0), Quaternion.identity);
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
                GameObject tile = Instantiate(tiles[3], new Vector3(0, 0), Quaternion.identity);
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
            for (int x = 0; x < newGridSize; x++) {
                GameObject tile;
                // Top Left
                if (x == 0) {
                    tile = Instantiate(tiles[6], new Vector3(0, 0), Quaternion.identity);
                }
                // Top Middle
                else if (x < gridSize) {
                    tile = Instantiate(tiles[7], new Vector3(0, 0), Quaternion.identity);
                }
                // Top Right
                else {
                    tile = Instantiate(tiles[8], new Vector3(0, 0), Quaternion.identity);
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
                for (int x = 0; x < newGridSize; x++) {
                    GameObject tile;
                    // Bottom Left
                    if (x == 0) {
                        tile = Instantiate(tiles[0], new Vector3(0, 0), Quaternion.identity);
                    }
                    // Bottom Middle
                    else if (x < gridSize) {
                        tile = Instantiate(tiles[1], new Vector3(0, 0), Quaternion.identity);
                    }
                    // Bottom Right
                    else {
                        tile = Instantiate(tiles[2], new Vector3(0, 0), Quaternion.identity);
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
                for (int x = 0; x < newGridSize; x++) {
                    GameObject tile;
                    // Bottom Left
                    if (x == 0) {
                        tile = Instantiate(tiles[0], new Vector3(0, 0), Quaternion.identity);
                    }
                    // Bottom Middle
                    else if (x < gridSize) {
                        tile = Instantiate(tiles[1], new Vector3(0, 0), Quaternion.identity);
                    }
                    // Bottom Right
                    else {
                        tile = Instantiate(tiles[2], new Vector3(0, 0), Quaternion.identity);
                    }
                    tile.name = gridReference[x][0];
                }
            }
        }
    }
    void DecreaseGridSize(int gridSize, int newGridSize, bool[] gridModification, float center) {
        // Destroys the column based on modification
        // Destroys the right
        if (gridModification[0]) {
            for (int y = 0; y < gridSize; y++) {
                Destroy(GameObject.Find(gridReference[newGridSize][y]));
            }
            // Destroys the left
        } else {
            for (int y = 0; y < gridSize; y++) {
                Destroy(GameObject.Find(gridReference[0][y]));
            }
        }
        // Destroys the row based on modification
        // Destroys the top
        if (gridModification[1]) {
            if (gridModification[0]) {
                for (int x = 0; x < newGridSize; x++) {
                    Destroy(GameObject.Find(gridReference[x][newGridSize]));
                }
            } else {
                for (int x = 1; x < gridSize; x++) {
                    Destroy(GameObject.Find(gridReference[x][newGridSize]));
                }
                for (int x = 1; x < gridSize; x++) {
                    for (int y = 0; y < newGridSize; y++) {
                        GameObject tile = GameObject.Find(gridReference[x][y]);
                        tile.transform.GetChild(0).name = x - 1 + "," + y;
                        tile.name = gridReference[x - 1][y];
                        FormatTiles(tile, center, newGridSize, x - 1, y);
                        UpdateTileSprites(tile, newGridSize, x - 1, y);
                    }
                }
            }
            // Destroys the bottom
        } else {
            if (gridModification[0]) {
                for (int x = 0; x < newGridSize; x++) {
                    Destroy(GameObject.Find(gridReference[x][0]));
                }
                for (int x = 0; x < newGridSize; x++) {
                    for (int y = 1; y < gridSize; y++) {
                        GameObject tile = GameObject.Find(gridReference[x][y]);
                        tile.transform.GetChild(0).name = x + "," + (y - 1);
                        tile.name = gridReference[x][y - 1];
                        FormatTiles(tile, center, newGridSize, x, y - 1);
                        UpdateTileSprites(tile, newGridSize, x, y - 1);
                    }
                }
            } else {
                for (int x = 1; x < gridSize; x++) {
                    Destroy(GameObject.Find(gridReference[x][0]));
                }
                for (int x = 1; x < gridSize; x++) {
                    for (int y = 1; y < gridSize; y++) {
                        GameObject tile = GameObject.Find(gridReference[x][y]);
                        tile.transform.GetChild(0).name = x - 1 + "," + (y - 1);
                        tile.name = gridReference[x - 1][y - 1];
                        FormatTiles(tile, center, newGridSize, x - 1, y - 1);
                        UpdateTileSprites(tile, newGridSize, x - 1, y - 1);
                    }
                }
            }
        }
    }
    public void UpdateGrid(int gridSize, int newGridSize, bool[] gridModification) {
        grid.transform.localScale = Vector3.one;
        float center = newGridSize / 2;
        if (newGridSize > gridSize) {
            IncreaseGridSize(gridSize, newGridSize, gridModification);
        } else {
            DecreaseGridSize(gridSize, newGridSize, gridModification, center);
        }
        for (int x = 0; x < newGridSize; x++) {
            for (int y = 0; y < newGridSize; y++) {
                FormatTiles(GameObject.Find(gridReference[x][y]), center, newGridSize, x, y);
                UpdateTileSprites(GameObject.Find(gridReference[x][y]), newGridSize, x, y);
            }
        }
        if (newGridSize > 1) {
            grid.transform.localScale = new Vector3(0.4f / Mathf.Log10(newGridSize), 0.4f / Mathf.Log10(newGridSize), 1);
        } else if (newGridSize == 1) {
            grid.transform.localScale = Vector3.one;
        }
    }
}
