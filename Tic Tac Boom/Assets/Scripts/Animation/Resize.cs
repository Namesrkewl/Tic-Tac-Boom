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
            Vector3 rescale = new Vector3(0.5f, 0.5f);
            gameObject.transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, new Vector3(rescale.x, rescale.y), 0.5f).setEaseOutElastic();
        }
    }
}
