using UnityEngine;
using UnityEngine.UI;

public class DisplayGems : MonoBehaviour {
    private int Gems;

    private void Start() {
        Gems = GameManager.instance.Gems;
        gameObject.GetComponent<Text>().text = Gems.ToString();
    }

    private void Update() {
        if (Gems != GameManager.instance.Gems) {
            Gems = GameManager.instance.Gems;
            gameObject.GetComponent<Text>().text = Gems.ToString();
        }
    }
}
