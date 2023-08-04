using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerObject")]
public class PlayerObject : ScriptableObject {
    public Sprite characterSprite, playerSprite;
    new public string name;
}
