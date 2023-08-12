using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour {
    public static GridManager instance;
    public Grid grid;
    public GameObject content, container;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Camera gridCamera;
    [field: SerializeField] public List<List<GameObject>> Tiles { get; private set; }

    public State state;
    public enum State {
        Idle,
        Generating,
        Changing,
        Formatting,
        Positioning,
        Tracking,
        Destroying
    }

    private void Awake() {
        instance = this;
    }

    public IEnumerator GenerateGrid(int size) {
        Tiles = new List<List<GameObject>>();
        for (int x = 0; x < size; x++) {
            Tiles.Add(new List<GameObject>());
            for (int y = 0; y < size; y++) {
                Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                Tiles[x].Add(_tile);
            }
        }
        state = State.Formatting;
        yield return StartCoroutine(FormatTiles(size));
        yield return null;
    }

    public IEnumerator DestroyGrid() {
        for (int x = Tiles.Count - 1; x >= 0; x--) {
            for (int y = Tiles[x].Count - 1; y >= 0; y--) {
                Destroy(Tiles[x][y]);
            }
            Tiles.RemoveAt(x);
        }
        state = State.Idle;
        yield return null;
    }

    public IEnumerator ResolveDraw() {
        for (int x = 0; x < Tiles.Count; x++) {
            for (int y = 0; y < Tiles.Count; y++) {
                Tiles[x][y].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                Tiles[x][y].transform.GetChild(0).gameObject.SetActive(false);
                Tiles[x][y].tag = "Untagged";
            }
        }
        state = State.Idle;
        yield return null;
    }

    public IEnumerator FormatTiles(int size) {
        string xString, yString;
        for (int x = 0; x < Tiles.Count; x++) {
            if (size == 1) {
                xString = "middle";
            } else if (x == 0) {
                xString = "left";
            } else if (x == (size - 1)) {
                xString = "right";
            } else {
                xString = "middle";
            }
            for (int y = 0; y < Tiles[x].Count; y++) {
                if (size == 1) {
                    yString = "middle";
                } else if (y == 0) {
                    yString = "bottom";
                } else if (y == (size - 1)) {
                    yString = "top";
                } else {
                    yString = "middle";
                }
                string tileset;
                switch (GameManager.instance.tileset) {
                    case GameManager.Tileset.Basic:
                        tileset = GameManager.Tileset.Basic.ToString();
                        break;
                    case GameManager.Tileset.Alternate:
                        tileset = GameManager.Tileset.Alternate.ToString();
                        break;
                    default:
                        tileset = null;
                        break;
                }
                Tiles[x][y].GetComponent<Tile>().SetSprite($"Tiles/{tileset}/{yString}-{xString}");
                Tiles[x][y].name = $"{x},{y}";
            }
        }
        state = State.Tracking;
        yield return StartCoroutine(CameraManager.instance.FollowGrid(size));
        yield return new WaitForSeconds(2f);
        state = State.Positioning;
        yield return StartCoroutine(ResetTilePosition(size));
        yield return null;
    }
    public IEnumerator ResetTilePosition(int size) {
        Debug.Log("ResetPositions");
        content.transform.localPosition = Vector3.zero;
        for (int x = 0; x < Tiles.Count; x++) {
            for (int y = 0; y < Tiles[x].Count; y++) {
                Tiles[x][y].transform.position = grid.GetCellCenterWorld(new Vector3Int(x, y));
            }
        }
        yield return StartCoroutine(CameraManager.instance.FollowGrid(size));
        yield return null;
    }
    public IEnumerator ChangeGridSize(int size, int newSize, Talent.Direction direction = Talent.Direction.TopRight) {
        Debug.Log(direction);
        if (size < newSize) {
            // Increase grid size
            switch (direction) {
                case Talent.Direction.TopRight:
                    for (int x = 0; x < size; x++) {
                        for (int y = size; y < newSize; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[x].Add(_tile);
                        }
                    }
                    for (int x = size; x < newSize; x++) {
                        Tiles.Add(new List<GameObject>());
                        for (int y = 0; y < newSize; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[x].Add(_tile);
                        }
                    }
                    break;
                case Talent.Direction.TopLeft:
                    for (int x = size - 1; x >= 0; x--) {
                        for (int y = size; y < newSize; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[x].Add(_tile);
                        }
                    }
                    for (int x = 0; x < newSize - size; x++) {
                        Tiles.Insert(0, new List<GameObject>());
                        for (int y = 0; y < newSize; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(-x - 1, y));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[0].Add(_tile);
                        }
                    }
                    content.transform.localPosition += new Vector3(grid.cellSize.x * (newSize - size), 0);
                    break;
                case Talent.Direction.BottomRight:
                    for (int x = 0; x < size; x++) {
                        for (int y = 0; y < newSize - size; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, -y - 1));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[x].Insert(0, _tile);
                        }
                    }
                    content.transform.localPosition += new Vector3(0, grid.cellSize.y * (newSize - size));
                    for (int x = size; x < newSize; x++) {
                        Tiles.Add(new List<GameObject>());
                        for (int y = 0; y < newSize; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[x].Add(_tile);
                        }
                    }
                    break;
                case Talent.Direction.BottomLeft:
                    for (int x = 0; x < size; x++) {
                        for (int y = 0; y < newSize - size; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, -y - 1));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[x].Insert(0, _tile);
                        }
                    }
                    content.transform.localPosition += new Vector3(0, grid.cellSize.y * (newSize - size));
                    for (int x = 0; x < newSize - size; x++) {
                        Tiles.Insert(0, new List<GameObject>());
                        for (int y = 0; y < newSize; y++) {
                            Vector3 position = grid.GetCellCenterWorld(new Vector3Int(-x - 1, y));
                            GameObject _tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                            Tiles[0].Add(_tile);
                        }
                    }
                    content.transform.localPosition += new Vector3(grid.cellSize.x * (newSize - size), 0);
                    break;
                default:
                    break;
            }
        } else if (size > newSize) {
            // Decrease grid size
            switch (direction) {
                case Talent.Direction.TopRight:
                    for (int x = size - 1; x >= newSize; x--) {
                        for (int y = 0; y < size; y++) {
                            Destroy(Tiles[x][y]);
                        }
                        Tiles.RemoveAt(x);
                    }
                    for (int x = 0; x < newSize; x++) {
                        for (int y = size - 1; y >= newSize; y--) {
                            Destroy(Tiles[x][y]);
                            Tiles[x].RemoveAt(y);
                        }
                    }
                    break;
                case Talent.Direction.TopLeft:
                    for (int x = 0; x < size - newSize; x++) {
                        for (int y = 0; y < size; y++) {
                            Destroy(Tiles[0][y]);
                        }
                        Tiles.RemoveAt(0);
                    }
                    for (int x = 0; x < newSize; x++) {
                        for (int y = size - 1; y >= newSize; y--) {
                            Destroy(Tiles[x][y]);
                            Tiles[x].RemoveAt(y);
                        }
                    }
                    break;
                case Talent.Direction.BottomRight:
                    for (int x = size - 1; x >= newSize; x--) {
                        for (int y = 0; y < size; y++) {
                            Destroy(Tiles[x][y]);
                        }
                        Tiles.RemoveAt(x);
                    }
                    for (int x = 0; x < newSize; x++) {
                        for (int y = 0; y < size - newSize; y++) {
                            Destroy(Tiles[x][0]);
                            Tiles[x].RemoveAt(0);
                        }
                    }
                    break;
                case Talent.Direction.BottomLeft:
                    for (int x = 0; x < size - newSize; x++) {
                        for (int y = 0; y < size; y++) {
                            Destroy(Tiles[0][y]);
                        }
                        Tiles.RemoveAt(0);
                    }
                    for (int x = 0; x < newSize; x++) {
                        for (int y = 0; y < size - newSize; y++) {
                            Destroy(Tiles[x][0]);
                            Tiles[x].RemoveAt(0);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        GameManager.instance.gridSize = GameManager.instance.newGridSize;
        state = State.Formatting;
        yield return StartCoroutine(FormatTiles(newSize));
        yield return null;
    }
}
