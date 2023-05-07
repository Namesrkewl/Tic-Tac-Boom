using UnityEngine;
using UnityEngine.UI;

public class DisplayGold : MonoBehaviour {
    private int Gold;

    private void Start() {
        Gold = GameManager.instance.Gold;
        gameObject.GetComponent<Text>().text = Gold.ToString();
    }

    private void Update() {
        if (Gold != GameManager.instance.Gold) {
            Gold = GameManager.instance.Gold;
            gameObject.GetComponent<Text>().text = Gold.ToString();
        }
    }
}
