using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour
{
    // Start is called before the first frame update
    public float newSize;
    private float newX, newY;

    private void OnEnable() {
        NewScale(newSize);
    }

    public void NewScale(float newSize) {
        if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "x" || gameObject.GetComponent<SpriteRenderer>().sprite.name == "o") {
            Vector3 rescale = new Vector3(0.8f, 0.8f);
            gameObject.transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, new Vector3(rescale.x, rescale.y), 0.5f).setEaseOutElastic();
        } else {
            newX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            newY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 rescale = gameObject.transform.localScale;
            rescale.x = newSize * rescale.x / newX;
            rescale.y = newSize * rescale.y / newY;
            gameObject.transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, new Vector3(rescale.x, rescale.y), 0.5f).setEaseOutElastic();
        }
    }
}
