using UnityEngine;
using TMPro;

public class DisplayGold : MonoBehaviour {
    private int Gold;

    private void Start() {
        Gold = GameManager.instance.Gold;
        gameObject.GetComponent<TextMeshProUGUI>().text = Gold.ToString();
    }

    private void Update() {
        if (Gold != GameManager.instance.Gold) {
            Gold = GameManager.instance.Gold;
            gameObject.GetComponent<TextMeshProUGUI>().text = Gold.ToString();
        }
    }
}
