using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerMoveTrigger))]
public class Tile : MonoBehaviour
{
    public GameObject occupant;

    public OccupantType occupantType;
    public enum OccupantType {
        None,
        Wall,
        Player,
        Enemy   
    }

    private void Start() {
        occupantType = OccupantType.None;
        SetBoxCollider();
        GenerateOccupant();
    }

    public void SetSprite(string path) {
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }

    private void SetBoxCollider() {
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(2.3f, 2.3f);
    }

    private void GenerateOccupant() {
        occupant = new GameObject("Occupant");
        occupant.SetActive(false);
        occupant.transform.SetParent(gameObject.transform);
        occupant.transform.localPosition = Vector3.zero;
        occupant.transform.localScale = new Vector3(0.8f, 0.8f);
        occupant.AddComponent<SpriteRenderer>();
        occupant.AddComponent<Resize>();
    }
}
