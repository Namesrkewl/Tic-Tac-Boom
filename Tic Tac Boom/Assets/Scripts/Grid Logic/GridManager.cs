using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject tile;

    private void Start() {
        grid = gameObject.GetComponent<Grid>();
        GenerateGrid(3);
    }

    public void GenerateGrid(int size) {
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                GameObject _tile = Instantiate(tile, position, Quaternion.identity, gameObject.transform);
                _tile.name = $"Tile {x},{y}";
                _tile.GetComponent<Tile>().SetSprite("Tiles/Basic/middle-middle");
                //_tile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Skins/Basic/x");
            }
        }
    }
}
