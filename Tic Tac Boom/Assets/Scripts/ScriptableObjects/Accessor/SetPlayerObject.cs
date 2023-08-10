using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetPlayerObject : MonoBehaviour
{
    public Image playerSprite, characterSprite;
    new public TextMeshProUGUI name;
    public PlayerObject playerObject;

    private void Update() {
        if (playerObject != null) {
            if (characterSprite.sprite != playerObject.characterSprite) {
                characterSprite.sprite = playerObject.characterSprite;
            }
            if (playerSprite.sprite != playerObject.playerSprite) {
                playerSprite.sprite = playerObject.playerSprite;
            }
            if (name.text != playerObject.name) {
                name.text = playerObject.name;
            }
        }
    }
}
