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

    private void OnEnable() {
        if (gameObject.scene.isLoaded) {
            /*
            ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1], GridManager.instance.container.transform);
            dust.transform.position = transform.position;
            dust.Play();
            */
        }
    }

    private void OnDestroy() {
        if (gameObject.scene.isLoaded) {
            /*
            ParticleSystem dust = Instantiate(ParticleSystemsManager.instance.particleSystems[1]);
            dust.transform.position = transform.position;
            dust.Play();
            */
        }
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
        occupant.AddComponent<SpriteRenderer>();
        occupant.AddComponent<Resize>();
    }
}
