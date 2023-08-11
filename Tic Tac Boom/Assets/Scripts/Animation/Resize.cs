using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable() {
        NewScale();
    }

    public void NewScale() {
        if (gameObject.GetComponent<SpriteRenderer>().sprite != null) {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "x" || gameObject.GetComponent<SpriteRenderer>().sprite.name == "o") {
                Vector3 rescale = new Vector3(0.8f, 0.8f);
                gameObject.transform.localScale = Vector3.zero;
                LeanTween.scale(gameObject, new Vector3(rescale.x, rescale.y), 0.5f).setEaseOutElastic();
            } else {
                Vector3 rescale = new Vector3(1f, 1f);
                gameObject.transform.localScale = Vector3.zero;
                LeanTween.scale(gameObject, new Vector3(rescale.x, rescale.y), 0.5f).setEaseOutElastic();
            }
        }
    }
}
