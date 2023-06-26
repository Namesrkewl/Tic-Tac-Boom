using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    Material material;

    private void Awake() {
        material = GetComponent<Renderer>().material;
    }

    public float speed = 0.1f;

    private void Update() {
        Vector2 offset = new Vector2(0, Time.deltaTime * speed);
        material.mainTextureOffset += offset;
    }
}
