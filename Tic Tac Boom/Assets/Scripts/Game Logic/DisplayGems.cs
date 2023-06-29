using UnityEngine;
using TMPro;

public class DisplayGems : MonoBehaviour {
    private int gems;

    private void Start() {
        gems = GameManager.instance.Gems;
        gameObject.GetComponent<TextMeshProUGUI>().text = gems.ToString();
    }

    private void Update() {
        if (gems != GameManager.instance.Gems) {
            gems = GameManager.instance.Gems;
            gameObject.GetComponent<TextMeshProUGUI>().text = gems.ToString();
        }
    }
}
